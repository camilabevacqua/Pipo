using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class IceGameManager : MonoBehaviour
{
    public static IceGameManager Instance;

    [Header("Configuración de Escena")]
    public GameObject grietaPrefab;
    public int poolSize = 5;
    public Transform spawnPoint;

    [Header("Configuración de Monedas")]
    public GameObject monedaPrefab;
    public int poolMonedasSize = 5;
    private List<GameObject> poolMonedas = new List<GameObject>();

    [Header("Dificultad")]
    public float spawnRate = 1.5f;
    public float velocidadObstaculos = 5f;

    [Header("UI")]
    public TextMeshProUGUI textScore;
    public TextMeshProUGUI textMonedas;
    public GameObject panelMenuInicio;

    [Header("UI GameOver")]
    public GameObject panelGameOver;
    public TextMeshProUGUI textFinalScore;
    public TextMeshProUGUI textFinalMonedas;

    [Header("Piposicion")]
    public GameObject pipo;

    private float[] carriles = { -1.5f, 0f, 1.5f };
    private int score = 0;
    private int monedasTotales = 0;

    private List<GameObject> poolGrietas = new List<GameObject>();
    private float nextSpawnTime;
    private bool juegoIniciado = false;

    void Awake()
    {
        Instance = this;
        Screen.orientation = ScreenOrientation.Portrait;

        // ESTADO INICIAL
        Time.timeScale = 0f;
        if (panelMenuInicio != null) panelMenuInicio.SetActive(true);
        if (panelGameOver != null) panelGameOver.SetActive(false);

        // pool de grietas
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(grietaPrefab);
            obj.SetActive(false);
            poolGrietas.Add(obj);
        }

        //pool de monedas
        for (int i = 0; i < poolMonedasSize; i++)
        {
            GameObject obj = Instantiate(monedaPrefab);
            obj.SetActive(false);
            poolMonedas.Add(obj);
        }
    }

    void Update()
    {
        if (juegoIniciado && Time.time >= nextSpawnTime)
        {
            SpawnObstaculos();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnObstaculos()
    {
        int indiceGrieta = Random.Range(0, carriles.Length);
        float xGrieta = carriles[indiceGrieta];

        GameObject grieta = GetGrietaDePool();
        if (grieta != null)
        {
            grieta.transform.position = new Vector3(xGrieta, spawnPoint.position.y, 0);
            grieta.SetActive(true);
        }

        if (Random.value > 0.5f)
        {
            int indiceMoneda = Random.Range(0, carriles.Length);
            while (indiceMoneda == indiceGrieta)
            {
                indiceMoneda = Random.Range(0, carriles.Length);
            }

            GameObject moneda = GetMonedaDePool();
            if (moneda != null)
            {
                moneda.transform.position = new Vector3(carriles[indiceMoneda], spawnPoint.position.y, 0);
                moneda.SetActive(true);
            }
        }
    }

    // --- FUNCIONES PARA BOTONES ---

    public void IniciarJuego() 
    {
        juegoIniciado = true;
        panelMenuInicio.SetActive(false);
        Time.timeScale = 1f;
    }

    public void VolverAlMenu()
    {
        ReiniciarJuego();
        juegoIniciado = false;
        Time.timeScale = 0f;
        panelMenuInicio.SetActive(true);
    }

    public void ReiniciarJuego()
    {
        juegoIniciado = true;
        panelGameOver.SetActive(false);
        Time.timeScale = 1f;

        //reset contadores
        score = 0;
        monedasTotales = 0;

        //reset texto ui
        if (textScore != null) textScore.text = "Score: 0";
        if (textMonedas != null) textMonedas.text = "Coins: 0";

        velocidadObstaculos = 5f;

        //limpiar grietas
        foreach (GameObject grieta in poolGrietas)
        {
            grieta.SetActive(false);
        }

        //limpiar monedas
        foreach (GameObject moneda in poolMonedas)
        {
            moneda.SetActive(false);
        }

        //reset pipo
        pipo.transform.position = new Vector3(0, pipo.transform.position.y, 0);
        pipo.GetComponent<PipoTiltController>().ResetearPosicion();

        IceHealthManager.Instance.ResetearVidas();
        nextSpawnTime = Time.time + spawnRate;
    }

    // --- LÓGICA DE JUEGO ---
    public void SumarPunto()
    {
        score++;
        if (textScore != null) textScore.text = "Score: " + score.ToString();

        // Cada 5 puntos aumenta la velocidad en 1
        if (score % 5 == 0)
        {
            velocidadObstaculos += 1f;
            Debug.Log("Velocidad aumentada a: " + velocidadObstaculos);
        }
    }

    public void SumarMoneda()
    {
        monedasTotales++;
        if (textMonedas != null) textMonedas.text = "Coins " + monedasTotales.ToString();
    }

    GameObject GetGrietaDePool()
    {
        foreach (GameObject obj in poolGrietas)
        {
            if (!obj.activeInHierarchy) return obj;
        }
        return null;
    }

    GameObject GetMonedaDePool()
    {
        foreach (GameObject obj in poolMonedas)
        {
            if (!obj.activeInHierarchy) return obj;
        }
        return null;
    }

    public void GameOver()
    {
        juegoIniciado = false;
        Time.timeScale = 0f;

        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);
        }

        if (textFinalScore != null)
            textFinalScore.text = "Final score: " + score.ToString();

        if (textFinalMonedas != null)
            textFinalMonedas.text = "Coins: " + monedasTotales.ToString();

    }

}
