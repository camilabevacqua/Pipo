using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Contenedores")]
    public GameObject uiStatsCasa;

    [Header("Barras de Estado")]
    public Image barraHambre;
    public Image barraEnergia;
    public Image barraFelicidad;
    public Image barraLimpieza;

    [Header("Economía")]
    public Text coinsText;

    [Header("Extras")]
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

        if (uiStatsCasa == null) uiStatsCasa = GameObject.Find("UI_Stats_Casa");

        bool esEscenaDeCasa = scene.name == "Bathroom" ||
                              scene.name == "Bedroom" ||
                              scene.name == "Patio" ||
                              scene.name == "Playground";
                              



        if (uiStatsCasa != null)
        {
            uiStatsCasa.SetActive(esEscenaDeCasa);
        }

        if (esEscenaDeCasa)
        {
            barraHambre = GameObject.Find("hambre")?.GetComponent<Image>();
            barraEnergia = GameObject.Find("energia")?.GetComponent<Image>();
            barraFelicidad = GameObject.Find("felicidad")?.GetComponent<Image>();
            barraLimpieza = GameObject.Find("limpieza")?.GetComponent<Image>();
            barraExp = GameObject.Find("barraExp")?.GetComponent<Slider>();
            nivelText = GameObject.Find("NivelText")?.GetComponent<Text>();

            var emotionsParent = GameObject.Find("Emotions");
            if (emotionsParent != null)
            {
                happy = emotionsParent.transform.Find("Happy")?.GetComponent<Image>();
                normal = emotionsParent.transform.Find("Normal")?.GetComponent<Image>();
                sad = emotionsParent.transform.Find("Sad")?.GetComponent<Image>();
                sick = emotionsParent.transform.Find("Sick")?.GetComponent<Image>();
            }
        }

        GameObject objCoins = GameObject.Find("CoinsText");
        if (objCoins != null) coinsText = objCoins.GetComponent<Text>();

        UpdateCoinsDisplay();
    }

    public void UpdateCoinsDisplay()
    {
        if (coinsText == null)
        {
            GameObject objCoins = GameObject.Find("CoinsText");
            if (objCoins != null) coinsText = objCoins.GetComponent<Text>();
        }

        if (coinsText != null)
        {
            coinsText.text = GameEconomy.GetCoins().ToString();
            Debug.Log("Monedas en UI actualizadas: " + coinsText.text);
        }
    }
}