using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private float rotationSpeed;

    void Start()
    {
        rotationSpeed = Random.Range(150f, 250f);

        if (Random.value > 0.5f) rotationSpeed *= -1;
    }

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}