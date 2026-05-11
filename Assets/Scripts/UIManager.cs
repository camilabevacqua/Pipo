using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Image barraHambre;
    public Image barraEnergia;
    public Image barraFelicidad;
    public Image barraLimpieza;

    [Header("Economía")]
    public Text coinsText; // Texto Legacy

    public Image happy;
    public Image normal;
    public Image sad;
    public Image sick;
    public Slider barraExp;
    public Text nivelText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            Destroy(gameObject);
            return;
        }

        // --- BUSQUEDA DE BARRAS ---
        barraHambre = GameObject.Find("hambre")?.GetComponent<Image>();
        barraEnergia = GameObject.Find("energia")?.GetComponent<Image>();
        barraFelicidad = GameObject.Find("felicidad")?.GetComponent<Image>();
        barraLimpieza = GameObject.Find("limpieza")?.GetComponent<Image>();
        barraExp = GameObject.Find("barraExp")?.GetComponent<Slider>();
        nivelText = GameObject.Find("NivelText")?.GetComponent<Text>();

        // --- BUSQUEDA DEL TEXTO DE MONEDAS (Crucial) ---
        // Buscamos el objeto por nombre en la escena actual para re-asignar la referencia
        GameObject objCoins = GameObject.Find("CoinsText");
        if (objCoins != null)
        {
            coinsText = objCoins.GetComponent<Text>();
        }

        var emotionsParent = GameObject.Find("Emotions");
        if (emotionsParent != null)
        {
            happy = emotionsParent.transform.Find("Happy")?.GetComponent<Image>();
            normal = emotionsParent.transform.Find("Normal")?.GetComponent<Image>();
            sad = emotionsParent.transform.Find("Sad")?.GetComponent<Image>();
            sick = emotionsParent.transform.Find("Sick")?.GetComponent<Image>();
        }

        UpdateCoinsDisplay();
    }

    public void UpdateCoinsDisplay()
    {
        // Si por alguna razón coinsText es nulo (ej: se borró el objeto), intentamos buscarlo de nuevo
        if (coinsText == null)
        {
            GameObject objCoins = GameObject.Find("CoinsText");
            if (objCoins != null) coinsText = objCoins.GetComponent<Text>();
        }

        if (coinsText != null)
        {
            coinsText.text = GameEconomy.GetCoins().ToString();
            Debug.Log("UI de Monedas actualizada: " + coinsText.text);
        }
    }
}