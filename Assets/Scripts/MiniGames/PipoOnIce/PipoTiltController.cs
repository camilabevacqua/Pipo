using UnityEngine;

public class PipoTiltController : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float sensibilidad = 15f;
    public float suavizado = 10f;
    public float limitePantallaX = 2.8f;

    private float posicionXObjetivo;

    void Update()
    {
        if (Time.timeScale == 0f) return;

        float inclinacion = Input.acceleration.x;

        posicionXObjetivo += inclinacion * sensibilidad * Time.deltaTime;

        posicionXObjetivo = Mathf.Clamp(posicionXObjetivo, -limitePantallaX, limitePantallaX);

        Vector3 nuevaPos = new Vector3(posicionXObjetivo, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, nuevaPos, Time.deltaTime * suavizado);
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
