using UnityEngine;

public class PinchZoom : MonoBehaviour
{
    [SerializeField] private RectTransform target;

    [SerializeField] private float zoomSpeed = 0.005f;

    [SerializeField] private float minZoom = 0.7f;
    [SerializeField] private float maxZoom = 2f;

    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 touch0Prev =
                touch0.position - touch0.deltaPosition;

            Vector2 touch1Prev =
                touch1.position - touch1.deltaPosition;

            float prevMagnitude =
                (touch0Prev - touch1Prev).magnitude;

            float currentMagnitude =
                (touch0.position - touch1.position).magnitude;

            float difference =
                currentMagnitude - prevMagnitude;

            Zoom(difference);
        }
    }

    void Zoom(float increment)
    {
        float scale =
            Mathf.Clamp(
                target.localScale.x + increment * zoomSpeed,
                minZoom,
                maxZoom
            );

        target.localScale =
            new Vector3(scale, scale, 1f);
    }
}