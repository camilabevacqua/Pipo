using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSource;

    public AudioClip levelUpSound;

    [Header("Mixer")]
    public AudioMixer mainMixer;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        DontDestroyOnLoad(gameObject);
    }

    public void SetBGMVolume(float value)
    {
        float dbValue =
            Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;

        mainMixer.SetFloat("VolumenBGM", dbValue);
    }

    public void SetSFXVolume(float value)
    {
        float dbValue =
            Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;

        mainMixer.SetFloat("VolumenSFX", dbValue);
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}