using UnityEngine;

public class FallDetector : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Plato") || col.gameObject.CompareTag("Ingrediente"))
        {
            gameObject.tag = "Ingrediente";

            var camara = Object.FindAnyObjectByType<FollowTower>();
            if (camara != null)
            {
                camara.ActualizarAltura(transform.position.y);
            }
        }
    }
}