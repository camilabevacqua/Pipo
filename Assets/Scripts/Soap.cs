using UnityEngine;

public class JabonDragEspuma : MonoBehaviour
{
    public GameObject espumaPrefab;
    public float intervalo = 0.5f;
    public float cantidadLimpieza = 3f;

    public AudioClip sonidoBañandose; 
    private AudioSource audioSource;

    private bool arrastrando = false;
    private bool tocandoPersonaje = false;
    private float temporizador = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = sonidoBañandose;
        audioSource.loop = true; 
        audioSource.playOnAwake = false;
    }

    void Update()
    {
    
        if (arrastrando)
        {
            Vector2 posMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = posMouse;
        }

        bool debeSonar = tocandoPersonaje && arrastrando && StatsPlayer.instance != null && StatsPlayer.instance.EstaEnBañera;

        if (debeSonar)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();

            temporizador += Time.deltaTime;
            if (temporizador >= intervalo)
            {
                if (espumaPrefab != null)
                    Instantiate(espumaPrefab, transform.position, Quaternion.identity);

                StatsPlayer.instance.Bañar(cantidadLimpieza);
                temporizador = 0f;
            }
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Stop();

            temporizador = 0f; 
        }
    }

    void OnMouseDown() => arrastrando = true;

    void OnMouseUp() => arrastrando = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tocandoPersonaje = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tocandoPersonaje = false;
            temporizador = 0f;
        }
    }
}