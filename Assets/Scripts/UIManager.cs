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

    public Text coinsText;

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
    public void UpdateCoinsDisplay()
    {
        if (coinsText == null)
            coinsText = GameObject.Find("CoinsText")?.GetComponent<Text>();

        if (coinsText != null)
            coinsText.text = GameEconomy.GetCoins().ToString();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateCoinsDisplay(); 
    }
}
