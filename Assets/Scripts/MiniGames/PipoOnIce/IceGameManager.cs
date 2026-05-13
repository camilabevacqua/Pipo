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

    [Header("Dificultad")]
    public float spawnRate = 1.5f;
    public float velocidadObstaculos = 5f;
    public TextMeshProUGUI textScore; 
    private int score = 0;

    [Header("UI")]
    public GameObject panelMenuInicio;
    public GameObject panelGameOver;

    [Header("Piposicion")]
    public GameObject pipo;

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

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(grietaPrefab);
            obj.SetActive(false);
            poolGrietas.Add(obj);
        }
    }

    void Update()
    {
        if (juegoIniciado && Time.time >= nextSpawnTime)
        {
            SpawnGrieta();
            nextSpawnTime = Time.time + spawnRate;
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

        score = 0;
        if (textScore != null) textScore.text = "Score: 0";
        velocidadObstaculos = 5f;

        foreach (GameObject grieta in poolGrietas)
        {
            grieta.SetActive(false);
        }

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

    void SpawnGrieta()
    {
        GameObject grieta = GetGrietaDePool();
        if (grieta != null)
        {
            float randomX = Random.Range(-2f, 2f);
            grieta.transform.position = new Vector3(randomX, spawnPoint.position.y, 0);
            grieta.SetActive(true);
        }
    }

    GameObject GetGrietaDePool()
    {
        foreach (GameObject obj in poolGrietas)
        {
            if (!obj.activeInHierarchy) return obj;
        }
        return null;
    }

    public void GameOver()
    {
        Time.timeScale = 0f;

        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);
        }

        Debug.Log("Game Over - Pipo se quedó sin vidas");
    }



}
