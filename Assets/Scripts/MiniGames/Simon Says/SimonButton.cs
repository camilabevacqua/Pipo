using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SimonButton : MonoBehaviour
{
    [Header("Referencias de UI")]
    [SerializeField] private Image normalImage; 
    [SerializeField] private Image highlightedImage; 
    [SerializeField] private Button interactionButton;

    [Header("Configuración")]
    [SerializeField] private int buttonIndex; 

    public int Index => buttonIndex;

    public Button Button => interactionButton;

    private void Awake()
    {
        SetHighlighted(false);
    }

    public void SetHighlighted(bool active)
    {
        if (highlightedImage != null)
        {
            highlightedImage.gameObject.SetActive(active);
        }

    }

    public IEnumerator Flash(float duration)
    {
        SetHighlighted(true);
        yield return new WaitForSeconds(duration);
        SetHighlighted(false);
    }
}