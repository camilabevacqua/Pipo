using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class StatsPlayer : MonoBehaviour
{
    public static StatsPlayer instance;

    [Header("Minijuegos")]
    [SerializeField] private string[] escenasMiniJuegos;

    [Range(0, 100)] public float hambre = 100f;
    [Range(0, 100)] public float energia = 100f;
    [Range(0, 100)] public float felicidad = 100f;
    [Range(0, 100)] public float limpieza = 100f;

    private bool estaEnCama = false;
    private bool estaEnBañera = false;
    public bool EstaEnBañera => estaEnBañera;

    [Header("Nivel / EXP")]
    private float exp = 0f;
    private int nivel = 1;

    public AudioClip levelUpSound;

    // Events
    private static event Action OnBedEnteredEvent;
    private static event Action OnBedExitedEvent;
    private static event Action OnBañeraEnteredEvent;
    private static event Action OnBañeraExitedEvent;

    public static void InvokeOnBedEnteredEvent() => OnBedEnteredEvent?.Invoke();
    public static void InvokeOnBedExitedEvent() => OnBedExitedEvent?.Invoke();
    public static void InvokeOnBañeraEnteredEvent() => OnBañeraEnteredEvent?.Invoke();
    public static void InvokeOnBañeraExitedEvent() => OnBañeraExitedEvent?.Invoke();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        Fruit.OnFruitCollected += HandleFruitCollected;

        OnBedEnteredEvent += () => estaEnCama = true;
        OnBedExitedEvent += () => estaEnCama = false;
        OnBañeraEnteredEvent += () => estaEnBañera = true;
        OnBañeraExitedEvent += () => estaEnBañera = false;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Fruit.OnFruitCollected -= HandleFruitCollected;

        OnBedEnteredEvent = null;
        OnBedExitedEvent = null;
        OnBañeraEnteredEvent = null;
        OnBañeraExitedEvent = null;
    }

    // =========================
    // STATS BASE
    // =========================
    void Update()
    {
        hambre = Mathf.Max(hambre - Time.deltaTime * 0.3f, 0);
        limpieza = Mathf.Max(limpieza - Time.deltaTime * 0.5f, 0);
        felicidad = Mathf.Max(felicidad - Time.deltaTime * 0.4f, 0);

        if (estaEnCama && !LampController.lampIsOn)
            energia = Mathf.Min(energia + Time.deltaTime * 0.8f, 100f);
        else
            energia = Mathf.Max(energia - Time.deltaTime * 0.8f, 0);

        RefreshUI();
    }

    // =========================
    // EXP SYSTEM
    // =========================

    float GetExpToNextLevel()
    {
        if (nivel < 5)
            return 10f;

        return 10f + (nivel * 4f);
    }

    public void AddExp(float amount)
    {
        exp += amount;

        while (exp >= GetExpToNextLevel())
        {
            exp -= GetExpToNextLevel();
            LevelUp();
        }

        RefreshUI();
    }

    void LevelUp()
    {
        nivel++;

        if (levelUpSound != null)
            AudioManager.instance?.PlaySound(levelUpSound);

        Debug.Log("Level Up! Nivel: " + nivel);
    }

    // =========================
    // UI
    // =========================

    void RefreshUI()
    {
        if (UIManager.instance == null) return;

        UIManager ui = UIManager.instance;

        ui.barraHambre.fillAmount = hambre / 100f;
        ui.barraEnergia.fillAmount = energia / 100f;
        ui.barraFelicidad.fillAmount = felicidad / 100f;
        ui.barraLimpieza.fillAmount = limpieza / 100f;

        float needed = GetExpToNextLevel();

        if (ui.barraExp != null)
        {
            ui.barraExp.maxValue = needed;
            ui.barraExp.value = exp;
        }

        if (ui.nivelText != null)
            ui.nivelText.text = nivel.ToString();

        EmotionState();
    }

    // =========================
    // EMOCIONES
    // =========================

    public void EmotionState()
    {
        if (UIManager.instance == null) return;

        UIManager ui = UIManager.instance;

        ui.happy.gameObject.SetActive(false);
        ui.normal.gameObject.SetActive(false);
        ui.sad.gameObject.SetActive(false);
        ui.sick.gameObject.SetActive(false);

        if (limpieza < 10f)
            ui.sick.gameObject.SetActive(true);
        else if (felicidad <= 30f)
            ui.sad.gameObject.SetActive(true);
        else if (felicidad > 70f)
            ui.happy.gameObject.SetActive(true);
        else
            ui.normal.gameObject.SetActive(true);
    }

    // =========================
    // STATS ACTIONS (CON XP)
    // =========================

    public void Comer(float cantidad)
    {
        hambre = Mathf.Clamp(hambre + cantidad, 0, 100);
        AddExp(0.6f);
    }

    public void Dormir(float cantidad) =>
        energia = Mathf.Clamp(energia + cantidad, 0, 100);

    public void Jugar(float cantidad) =>
        felicidad = Mathf.Clamp(felicidad + cantidad, 0, 100);

    public void Bañar(float cantidad)
    {
        limpieza = Mathf.Clamp(limpieza + cantidad, 0, 100);
    }

    private void HandleFruitCollected(float cantidadHambre)
    {
        Comer(cantidadHambre);
    }

    // =========================
    // SCENES
    // =========================

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool esMiniJuego = false;

        foreach (string nombre in escenasMiniJuegos)
        {
            if (scene.name == nombre)
            {
                esMiniJuego = true;
                break;
            }
        }

        SetPipoVisible(!esMiniJuego);
        StartCoroutine(WaitForUIReady());
    }

    System.Collections.IEnumerator WaitForUIReady()
    {
        yield return new WaitForSeconds(0.5f);
        RefreshUI();
    }

    void SetPipoVisible(bool visible)
    {
        Transform pipoVisual = transform.Find("Pipo");

        if (pipoVisual != null)
            pipoVisual.gameObject.SetActive(visible);
    }
}