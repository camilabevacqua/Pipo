using UnityEngine;
using System;

public abstract class Fruit : MonoBehaviour, ICollectible
{
    public float cantidadHambre = 10f;

    private bool arrastrando = false;

    public static event Action<float> OnFruitCollected;
    private float tiempoVida = 10f;
    public AudioClip spawnSound;
    public AudioClip eatSound;

    void Start()
    {
        if (spawnSound != null && SFXManager.instance != null)
        {
            SFXManager.instance.PlaySFX(spawnSound);
        }
    }
    public virtual void Collect()
    {
        if (eatSound != null && SFXManager.instance != null)
        {
            SFXManager.instance.PlaySFX(eatSound);
        }
        OnFruitCollected?.Invoke(cantidadHambre);
        Destroy(gameObject);
    }

    void Update()
    {
        if (arrastrando)
        {
            Vector2 posMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = posMouse;
        }
        tiempoVida -= Time.deltaTime;
        if (tiempoVida <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnMouseDown()
    {
        arrastrando = true;
    }

    void OnMouseUp()
    {
        arrastrando = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }
}