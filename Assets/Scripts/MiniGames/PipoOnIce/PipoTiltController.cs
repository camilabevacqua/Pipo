using UnityEngine;

public class PipoTiltController : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float sensibilidad = 15f;
    public float suavizado = 10f;

    private Rigidbody2D rb;
    private float posicionXObjetivo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        posicionXObjetivo = transform.position.x;
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        float inclinacion = Input.acceleration.x;
        posicionXObjetivo += inclinacion * sensibilidad * Time.deltaTime;
    }

    void FixedUpdate()
    {
        Vector2 posicionActual = rb.position;
        Vector2 objetivo = new Vector2(posicionXObjetivo, rb.position.y);

        Vector2 nuevaPos = Vector2.Lerp(posicionActual, objetivo, Time.fixedDeltaTime * suavizado);

        rb.MovePosition(nuevaPos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Grieta"))
        {
            IceHealthManager.Instance.RecibirDanio();
            collision.gameObject.SetActive(false);
        }
    }

    public void ResetearPosicion()
    {
        posicionXObjetivo = 0f;
    }
}
