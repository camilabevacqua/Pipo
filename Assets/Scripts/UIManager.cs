using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Audio")]
    public AudioMixer mainMixer;

    [Header("Contenedores")]
    public GameObject uiStatsCasa;
    public GameObject panelAjustes;
    public GameObject panelPrincipalOpciones;

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

    [Header("Sliders Audio")]
    public Slider sliderBGM;
    public Slider sliderSFX;

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

    void Start()
    {
        float bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        SetVolumeBGM(bgmVolume);
        SetVolumeSFX(sfxVolume);

        if (sliderBGM != null)
            sliderBGM.value = bgmVolume;

        if (sliderSFX != null)
            sliderSFX.value = sfxVolume;
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

        if (uiStatsCasa == null)
            uiStatsCasa = GameObject.Find("UI_Stats_Casa");

        bool esEscenaDeCasa =
            scene.name == "Bathroom" ||
            scene.name == "Bedroom" ||
            scene.name == "Patio" ||
            scene.name == "Playground" ||
            scene.name == "Casa";

        if (uiStatsCasa != null)
        {
            uiStatsCasa.SetActive(esEscenaDeCasa);
        }

        if (!esEscenaDeCasa)
        {
            CerrarTodo();
        }

        if (esEscenaDeCasa)
        {
            RevincularComponentesCasa();
        }

        GameObject objCoins = GameObject.Find("CoinsText");

        if (objCoins != null)
            coinsText = objCoins.GetComponent<Text>();

        UpdateCoinsDisplay();
    }

    
    public void SetVolumeSFX(float value)
    {
        float dbValue = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;

        mainMixer.SetFloat("VolumenSFX", dbValue);

        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
    }

    public void SetVolumeBGM(float value)
    {
        float dbValue = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;

        mainMixer.SetFloat("VolumenBGM", dbValue);

        PlayerPrefs.SetFloat("BGMVolume", value);
        PlayerPrefs.Save();
    }

    
    public void AbrirMenuOpciones()
    {
        if (panelPrincipalOpciones != null)
            panelPrincipalOpciones.SetActive(true);
    }

    public void AbrirAjustesAudio()
    {
        if (panelAjustes != null)
            panelAjustes.SetActive(true);
    }

    public void VolverDeAjustes()
    {
        if (panelAjustes != null)
            panelAjustes.SetActive(false);

        if (panelPrincipalOpciones != null)
            panelPrincipalOpciones.SetActive(true);
    }

    public void CerrarTodo()
    {
        if (panelPrincipalOpciones != null)
            panelPrincipalOpciones.SetActive(false);

        if (panelAjustes != null)
            panelAjustes.SetActive(false);
    }

   

    void RevincularComponentesCasa()
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

    public void UpdateCoinsDisplay()
    {
        if (coinsText == null)
        {
            GameObject objCoins = GameObject.Find("CoinsText");

            if (objCoins != null)
                coinsText = objCoins.GetComponent<Text>();
        }

        if (coinsText != null)
        {
            coinsText.text = GameEconomy.GetCoins().ToString();
        }
    }
}