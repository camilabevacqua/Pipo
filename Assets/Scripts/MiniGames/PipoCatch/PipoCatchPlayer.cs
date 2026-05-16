using UnityEngine;

public class PipoCatchPlayer : MonoBehaviour
{
    private Rigidbody2D rb;
    private Camera cam;

    [SerializeField] private float speed = 10f;
    [SerializeField] private float touchSpeed = 20f;

    private bool agarrado = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
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
        float move = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        if (agarrado)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            Vector2 posicionObjetivo =
                new Vector2(mousePos.x, rb.position.y);

            Vector2 nuevaPosicion =
                Vector2.Lerp(
                    rb.position,
                    posicionObjetivo,
                    Time.fixedDeltaTime * touchSpeed
                );

            rb.MovePosition(nuevaPosicion);
        }
    }
}