using System.Collections;
using System.Collections.Generic;
using ReZeros.Jaxer.Util;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    private Transform currentEffectsObject;
    private Transform currentEffectsParent;

    private List<ParticleSystem> effects;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        effects = new List<ParticleSystem>();
    }

    public void PlayOneShot(ParticleSystem particleSystem, Vector3 position)
    {
        if (particleSystem == null) return;

        var effect = Instantiate(particleSystem, position, Quaternion.identity);
        effect.Play();

        var duration = effect.main.duration + effect.main.startLifetime.constantMax;
        effect.gameObject.AddComponent<Disposable>().lifetime = duration;
    }

    public void PlaySpriteOneShot(SpriteRenderer spriteEffect, Vector3 position, bool flipX)
    {
        var obj = Instantiate(spriteEffect, position, Quaternion.identity);
        obj.flipX = flipX;
        obj.gameObject.AddComponent<Disposable>().lifetime = 2f;
    }

    private class EffectPool
    {
        private const int PoolSize = 5;

        private List<ParticleSystem> effectPool;
        private int currentEffectIndex;

        public EffectPool(ParticleSystem particleSystem)
        {
            var pMain = particleSystem.main;
            pMain.playOnAwake = false;

            effectPool = new List<ParticleSystem>();
            for (int i = 0; i < PoolSize; i++)
            {
                effectPool.Add(Instantiate(particleSystem, EffectManager.Instance.transform));
            }
        }

        public void Play(Vector3 position)
        {
            var effect = effectPool[currentEffectIndex];
            effect.transform.position = position;
            effect.Play();

            currentEffectIndex = (currentEffectIndex + 1) % effectPool.Count;
        }

        public void PlayWithColor(Vector3 position, Color color)
        {
            var effect = effectPool[currentEffectIndex];

            // Temporarily override start color
            var main = effect.main;
            var prevColor = main.startColor;
            main.startColor = color;

            Play(position);

            EffectManager.Instance.StartCoroutine(ResetEffectColor(main, prevColor, main.duration));
        }

        private IEnumerator ResetEffectColor(ParticleSystem.MainModule system, ParticleSystem.MinMaxGradient color,
            float delay)
        {
            yield return new WaitForSeconds(delay);
            system.startColor = color;
        }
    }
}