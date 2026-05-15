using UnityEngine;
using UnityEngine.SceneManagement;

public class TabletInteractor : MonoBehaviour
{
    [Header("Referencias de UI")]
    [SerializeField] private GameObject minigamesPanel;

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

    public void LoadSimonSaysScene()
    {
        SceneManager.LoadScene("SimonSays");
    }
    public void LoadPipoCatchScene()
    {
        SceneManager.LoadScene("PipoCatch");
    }
    public void LoadMemoryGameScene()
    {
        SceneManager.LoadScene("MemoryGame");
    }
    public void LoadPipoOnIceScene()
    {
        SceneManager.LoadScene("PipoOnIce");
    }
    public void LoadMinesweeperScene()
    {
        SceneManager.LoadScene("Minesweeper");
    }
}