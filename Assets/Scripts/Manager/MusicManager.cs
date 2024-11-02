using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

	[SerializeField]
	private MusicLibrary musicLibrary;
	[SerializeField]
    private AudioSource musicSource;

	public string currentTrackName;

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	private void OnEnable()
	{
		// 订阅场景加载事件
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable()
	{
		// 取消订阅场景加载事件，避免内存泄漏
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		string sceneName = scene.name;
		PlayMusic(sceneName);
	}


	public void PlayMusic(string trackName, float fadeDuration = 0.5f)
	{
		if (string.Equals(trackName, "Persistence", StringComparison.Ordinal))
		{
			return;
		}
		StartCoroutine(AnimateMusicCrossFade(trackName, fadeDuration));
	}

	IEnumerator AnimateMusicCrossFade(string trackName, float fadeDuration = 0.5f)
	{
		AudioClip nextTrack = musicLibrary.GetClipFromName(trackName);
		float percent = 0;
		while (percent < 1)
		{
			percent += Time.deltaTime * 1 / fadeDuration;
			musicSource.volume = Mathf.Lerp(1f, 0, percent);
			yield return null;
		}
		
		musicSource.clip = nextTrack;
		currentTrackName = trackName;
		musicSource.Play();

		percent = 0;
		while (percent < 1)
		{
			percent += Time.deltaTime * 1 / fadeDuration;
			musicSource.volume = Mathf.Lerp(0, 1f, percent);
			yield return null;
		}
	}
}
