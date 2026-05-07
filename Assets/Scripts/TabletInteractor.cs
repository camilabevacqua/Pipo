using UnityEngine;
using UnityEngine.SceneManagement;

public class TabletInteractor : MonoBehaviour
{
    [Header("Referencias de UI")]
    [SerializeField] private GameObject minigamesPanel;

    // Se ejecuta cuando hacés clic en el objeto (requiere Collider 2D)
    private void OnMouseDown()
    {
        AbrirMenu();
    }

    public void AbrirMenu()
    {
        if (minigamesPanel != null)
        {
            minigamesPanel.SetActive(true);
        }
    }

    public void CerrarMenu()
    {
        if (minigamesPanel != null)
        {
            minigamesPanel.SetActive(false);
        }
    }

    // Función para el botón del Simon Says
    public void LoadSimonSaysScene()
    {
        // Reemplazá "SimonSaysScene" por el nombre exacto de tu escena del juego
        SceneManager.LoadScene("SimonSays");
    }
}