using UnityEngine;
using UnityEngine.UI;

public class IceHealthManager : MonoBehaviour
{
    public static IceHealthManager Instance;

    [Header("Configuración de Vidas")]
    public int vidasMaximas = 3;
    private int vidasActuales;

    [Header("UI - Corazones")]
    public Image[] corazonesUI;

    void Awake()
    {
        Instance = this;
        vidasActuales = vidasMaximas;
    }

    public void RecibirDanio()
    {
        if (vidasActuales <= 0) return;

        vidasActuales--;

        ActualizarCorazones();

        if (vidasActuales <= 0)
        {
            IceGameManager.Instance.GameOver();
        }
    }

    void ActualizarCorazones()
    {
        for (int i = 0; i < corazonesUI.Length; i++)
        {
            if (i < vidasActuales)
            {
                corazonesUI[i].gameObject.SetActive(true);
            }
            else
            {
                corazonesUI[i].gameObject.SetActive(false);
            }
        }
    }

    public void ResetearVidas()
    {
        vidasActuales = vidasMaximas;
        ActualizarCorazones();
    }
}
