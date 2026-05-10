using UnityEngine;

public class CatchItem : MonoBehaviour
{
    [SerializeField] private bool esBueno;
    [SerializeField] private int puntosQueDa = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (esBueno)
            {
                GameManagerCatch.instance.SumarPuntos(puntosQueDa);
            }
            else
            {
                GameManagerCatch.instance.PerderVida();
            }
            Destroy(gameObject);
        }

        if (collision.CompareTag("Suelo"))
        {
            Destroy(gameObject);
        }
    }
}
