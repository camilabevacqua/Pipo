using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManagerCatch : MonoBehaviour
{
    [Header("Recompensas")]
    [SerializeField] private TextMeshProUGUI textoMonedasGanadas;

    private int monedasGanadas = 0;

    [Header("Economía")]
    [SerializeField] private int puntosPorRecompensa = 100;
    [SerializeField] private int monedasPorRecompensa = 5;

    private int siguienteRecompensa = 100;

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

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Time.timeScale = 0;

        panelInicio.SetActive(true);
        panelGameOver.SetActive(false);

        if (textoMonedasGanadas != null)
        {
            textoMonedasGanadas.gameObject.SetActive(false);
        }

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

        if (puntos >= siguienteRecompensa)
        {
            monedasGanadas += monedasPorRecompensa;

            GameEconomy.AddCoins(monedasPorRecompensa);

            Debug.Log("Ganaste " + monedasPorRecompensa + " monedas!");

            StartCoroutine(
                MostrarTextoMonedas(
                    "+" + monedasPorRecompensa + " monedas!"
                )
            );

            siguienteRecompensa += puntosPorRecompensa;
        }
    }

    IEnumerator MostrarTextoMonedas(string mensaje)
    {
        if (textoMonedasGanadas == null)
            yield break;

        textoMonedasGanadas.text = mensaje;

        textoMonedasGanadas.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(1.5f);

        textoMonedasGanadas.gameObject.SetActive(false);
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

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    public void BotonExit()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene("Playground");
    }

    public void TerminarJuego()
    {
        Time.timeScale = 0;

        float expGanada = puntos / 120f;

        expGanada += 2f;

        expGanada = Mathf.Clamp(expGanada, 2f, 12f);

        StatsPlayer.instance?.AddExp(expGanada);

        if (textoPuntajeFinal != null)
        {
            textoPuntajeFinal.text =
                "Puntaje Final: " + puntos +
                "\nMonedas Ganadas: " + monedasGanadas;
        }

        panelGameOver.SetActive(true);
    }

    public void BotonMenu()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }
}