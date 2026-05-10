using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManagerCatch : MonoBehaviour
{
    public static GameManagerCatch instance;
    public TextMeshProUGUI textoPuntajeFinal;

    [Header("Estadísticas")]
    public int vidas = 3;
    public int puntos = 0;

    [Header("Referencias UI")]
    public TextMeshProUGUI textoPuntos;
    public GameObject panelInicio;
    public GameObject panelGameOver;
    public GameObject[] corazones;

    void Awake() { instance = this; }

    void Start()
    {
        Time.timeScale = 0;
        panelInicio.SetActive(true);
        panelGameOver.SetActive(false);
        ActualizarTextoPuntos();
    }

    public void BotonPlay()
    {
        panelInicio.SetActive(false);
        Time.timeScale = 1;
    }

    public void BotonBack()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Playground");
    }

    public void SumarPuntos(int cantidad)
    {
        puntos += cantidad;
        ActualizarTextoPuntos();
    }

    void ActualizarTextoPuntos()
    {
        textoPuntos.text = "Score: " + puntos;
    }

    public void PerderVida()
    {
        if (vidas > 0)
        {
            vidas--;
            corazones[vidas].SetActive(false);

            if (vidas <= 0)
            {
                TerminarJuego();
            }
        }
    }
    public void BotonRetry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void BotonExit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Playground");
    }

    public void TerminarJuego()
    {
        Time.timeScale = 0;

        if (textoPuntajeFinal != null)
        {
            textoPuntajeFinal.text = "Puntaje Final: " + puntos.ToString();
        }

        panelGameOver.SetActive(true);
    }
}
