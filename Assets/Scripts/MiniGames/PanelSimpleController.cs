using UnityEngine;

public class PanelSimpleController : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject panel;
    public bool pausarJuego = true;

    public void AbrirPanel()
    {
        if (panel != null)
        {
            panel.SetActive(true);

            if (pausarJuego)
            {
                Time.timeScale = 0f;
            }
        }
    }

    public void CerrarPanel()
    {
        if (panel != null)
        {
            panel.SetActive(false);

            if (pausarJuego)
            {
                Time.timeScale = 1f;
            }
        }
    }
}
