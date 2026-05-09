using UnityEngine;

public class FollowTower : MonoBehaviour
{
    public float suavizado = 2f;
    private float alturaMaxima = 0f;

    public void ActualizarAltura(float nuevaAltura)
    {
        if (nuevaAltura > alturaMaxima) alturaMaxima = nuevaAltura;
    }

    void Update()
    {
        Vector3 puntoObjetivo = new Vector3(transform.position.x, alturaMaxima + 2f, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, puntoObjetivo, suavizado * Time.deltaTime);
    }
}