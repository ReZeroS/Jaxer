using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] private AudioSource[] sfxList;
    [SerializeField] private AudioSource[] bgmList;
    [SerializeField] private float minumDistanceForSFX;
    
    
    public static AudioManager instance;

    public bool playBgm;
    private int bgmIndex;
    
    public bool canPlaySFX;
    
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Invoke(nameof(AllowPlaySFX), 1f);
    }

    private void Update()
    {
        if (!playBgm)
        {
            StopAllBGM();
        }
        else
        {
            if (!bgmList[bgmIndex].isPlaying)
            {
                PlayBGM(bgmIndex);
            }
        }
    }


    public void PlaySFX(int index)
    {
        PlaySFX(index, null);
    }
    
    public void PlaySFX(int index, Transform source)
    {
        if (!canPlaySFX)
        {
            return;
        }
        if (index >= sfxList.Length)
        {
            return;
        }
        // if (sfxList[index].isPlaying)
        // {
        //     return;
        // }

        Vector3 playerPos = PlayerManager.instance.player.transform.position;
        if (source && Vector2.Distance(source.position, playerPos) > minumDistanceForSFX)
        {
            return;
        }
        sfxList[index].pitch = Random.Range(0.8f, 1.2f);
        sfxList[index].Play();
    }
    public void StopSFX(int index)
    {
        if (index < sfxList.Length)
        {
            sfxList[index].Stop();
        }
    }
    
    public void PlayBGM(int index)
    {
        bgmIndex = index;
        StopAllBGM();
        if (index < bgmList.Length)
        {
            bgmList[bgmIndex].Play();
        }
    }

    private void StopAllBGM()
    {
        foreach (AudioSource bgm in bgmList)
        {
            bgm.Stop();
        }
    }

    public void StopBGM(int index)
    {
        if (index < bgmList.Length)
        {
            bgmList[index].Stop();
        }
    }
    
    
    private void AllowPlaySFX() => canPlaySFX = true;


    public void FadeOutSfxTime(int index, float time)
    {
        StartCoroutine(FadeOut(sfxList[index], time));
    }
    
    public void FadeInSfxTime(int index, float time)
    {
        StartCoroutine(FadeIn(sfxList[index], time));
    }
    
    
    private IEnumerator FadeIn(AudioSource audioSource, float duration)
    {
        float startVolume = 0.0f;
        float targetVolume = audioSource.volume;

        audioSource.volume = startVolume;
        audioSource.Play();

        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += (Time.deltaTime / duration) * targetVolume;
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    private IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;
        float targetVolume = 0.0f;

        while (audioSource.volume > targetVolume)
        {
            audioSource.volume -= (Time.deltaTime / duration) * startVolume;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
    
    
    
}
