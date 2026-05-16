using UnityEngine;

public class IceCoin : MonoBehaviour
{
    void Update()
    {
        transform.Translate(Vector2.up * IceGameManager.Instance.velocidadObstaculos * Time.deltaTime);

        if (transform.position.y > 6f)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            IceGameManager.Instance.SumarMoneda();

            gameObject.SetActive(false);
        }
    }
}
