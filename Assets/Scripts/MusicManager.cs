using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] AudioClip menuClip;
    [SerializeField] AudioClip level1Clip;
    [SerializeField] AudioClip level2Clip;
    [SerializeField] AudioClip level3Clip;
    [SerializeField] AudioClip level4Clip;
    [SerializeField] AudioClip BossFightClip;

    [SerializeField] private float targetVolume = 0.5f;
    [SerializeField] private float fadeDuration = 1f;

    AudioSource audioSource;
    Coroutine fadeRoutine;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.spatialBlend = 0f;
        audioSource.volume = 0f; // fade handles raising it, including on the very first track
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void Start()
    {
        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    void PlayMusicForScene(string sceneName)
    {
        AudioClip clip = sceneName switch
        {
            "Menu" => menuClip,
            "Level 1" => level1Clip,
            "Level 2" => level2Clip,
            "Level 3" => level3Clip,
            "Level 4" => level4Clip,
            _ => null
        };

        if (clip == null || audioSource == null)
        {
            return;
        }

        if (audioSource.clip == clip && audioSource.isPlaying)
        {
            return;
        }

        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(SwapClipWithFade(clip));
    }

    IEnumerator SwapClipWithFade(AudioClip newClip)
    {
        if (audioSource.isPlaying)
        {
            yield return FadeVolume(audioSource.volume, 0f, fadeDuration);
        }

        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.loop = true;
        audioSource.Play();

        yield return FadeVolume(0f, targetVolume, fadeDuration);
    }

    IEnumerator FadeVolume(float from, float to, float duration)
    {
        if (audioSource == null) yield break;
        const float maxStep = 0.05f; // caps a scene-load stall frame, same fix as ScreenFader

        audioSource.volume = from;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (audioSource == null) yield break;
            elapsed += Mathf.Min(Time.unscaledDeltaTime, maxStep);
            audioSource.volume = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        if (audioSource != null) audioSource.volume = to;
    }
}