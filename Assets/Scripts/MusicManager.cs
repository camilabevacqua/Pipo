using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;

    private AudioSource audioSource;

    private readonly int[] escenasConMusica = { 1, 2, 3, 4 };

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int sceneIndex = scene.buildIndex;

        if (sceneIndex == 0)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
        else if (System.Array.Exists(escenasConMusica, id => id == sceneIndex))
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
    }

    public void SetBGMVolume(float volume)
    {
        if (audioSource != null) audioSource.volume = volume;
    }
}