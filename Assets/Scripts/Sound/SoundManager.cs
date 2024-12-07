using System;
using ReZeros.Jaxer.Util;
using UnityEngine;
using UnityEngine.Audio;

namespace Sound.SoundManager
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private SoundsSO SO;
        public static SoundManager instance;
        private AudioSource audioSource;

        private void Awake()
        {
            if (!instance)
            {
                instance = this;
                audioSource = GetComponent<AudioSource>();
            }
        }

        public static void PlaySound(SoundType sound, float volume = 1)
        {
            if (!instance)
            {
                return;
            }

            SoundList soundList = instance.SO.sounds[(int)sound];
            AudioClip[] clips = soundList.sounds;
            AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

            instance.audioSource.outputAudioMixerGroup = soundList.mixer;
            instance.audioSource.PlayOneShot(randomClip, volume * soundList.volume);
        }

        public static void PlaySound3d(SoundType sound, Transform trans = default, float volume = 1)
        {
            SoundList soundList = instance.SO.sounds[(int)sound];
            AudioClip[] clips = soundList.sounds;
            AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

            instance.audioSource.outputAudioMixerGroup = soundList.mixer;
            instance.audioSource.PlayOneShot(randomClip, volume * soundList.volume);
        }

        public static void StopSound(AudioSource source = null)
        {
            if (source)
            {
                source.Stop();
            }
            else
            {
                instance.audioSource.Stop();
            }
        }


        public void PlaySoundAtLocation(AudioClip clip, Vector3 position, float volume = 1.0f)
        {
            if (clip == null) return;

            var tempAudioSource = new GameObject("TempAudio")
            {
                transform =
                {
                    position = position
                }
            };
            var src = tempAudioSource.AddComponent<AudioSource>();
            src.clip = clip;
            src.volume = volume;
            src.spatialBlend = 1.0f;
            src.rolloffMode = AudioRolloffMode.Linear;
            src.minDistance = 5.0f;
            src.maxDistance = 15.0f;
            src.dopplerLevel = 0;
            tempAudioSource.AddComponent<Disposable>().lifetime = clip.length;
            src.Play();
        }
    }

    [Serializable]
    public struct SoundList
    {
        [HideInInspector] public string name;
        [Range(0, 1)] public float volume;
        public AudioMixerGroup mixer;
        public AudioClip[] sounds;
    }
}