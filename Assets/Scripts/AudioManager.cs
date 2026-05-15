using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSource;

    public AudioClip levelUpSound;

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

    public void SetSFXVolume(float valor)
    {
        if (audioSource != null)
        {
            audioSource.volume = valor;
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}