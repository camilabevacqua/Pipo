using UnityEngine;

public class PipoTouchMovement : MonoBehaviour
{
    private Camera cam;
    private Rigidbody2D rb;
    private bool agarrado = false;

    [Header("Configuración de Arrastre")]
    public float suavizado = 20f;

    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 puntoToque = cam.ScreenToWorldPoint(Input.mousePosition);
            if (GetComponent<Collider2D>().OverlapPoint(puntoToque))
            {
                agarrado = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            agarrado = false;
        }
    }

    void FixedUpdate()
    {
        if (agarrado)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            Vector2 posicionObjetivo = new Vector2(mousePos.x, rb.position.y);

            Vector2 nuevaPosicion = Vector2.Lerp(rb.position, posicionObjetivo, Time.fixedDeltaTime * suavizado);

            rb.MovePosition(nuevaPosicion);
        }
    }
}
