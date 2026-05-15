using UnityEngine;

public class GrietaBehavior : MonoBehaviour
{
    private bool yaPuntuo = false;

    void OnEnable()
    {
        yaPuntuo = false;
    }

    void Update()
    {
        transform.Translate(Vector2.up * IceGameManager.Instance.velocidadObstaculos * Time.deltaTime);

        if (!yaPuntuo && transform.position.y > 0f)
        {
            yaPuntuo = true;
            IceGameManager.Instance.SumarPunto();
        }

        if (transform.position.y > 6f)
        {
            gameObject.SetActive(false);
        }
    }
}
