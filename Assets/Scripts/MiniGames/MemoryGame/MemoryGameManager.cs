using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class MemoryGameManager : MonoBehaviour
{
    public static MemoryGameManager Instance;

    [Header("Referencias")]
    public Transform gridPadre;
    public GameObject cardPrefab;
    public TextMeshProUGUI textTiempo;
    public CanvasGroup gridCanvasGroup;
    public TextMeshProUGUI textCuentaRegresiva;

    [Header("Sprites de Cartas")]
    public Sprite dorso;
    public Sprite[] iconos; // Acá van 20 dibujos diferentes

    private MemoryLevel nivelActual;
    private List<MemoryCard> cartas = new List<MemoryCard>();
    private MemoryCard primera, segunda;
    private float cronometro;
    private int paresEncontrados;
    private bool juegoIniciado = false;

    [Header("Paneles")]
    public GameObject panelMenu;
    public GameObject panelJuego;

    [Header("Paneles de Resultado")]
    public GameObject panelVictoria;
    public GameObject panelDerrota;
    public TextMeshProUGUI textMonedasVictoria;

    void Awake() => Instance = this;

    public void SeleccionarDificultad(MemoryLevel dificultadElegida)
    {
        panelMenu.SetActive(false);
        panelJuego.SetActive(true);

        EmpezarJuego(dificultadElegida);
        AjustarTamañoGrid(dificultadElegida);
    }

    public void EmpezarJuego(MemoryLevel nivel)
    {
        nivelActual = nivel;
        LimpiarTablero();
        PrepararCartas();
        StartCoroutine(SecuenciaInicial());

    }

    void PrepararCartas()
    {
        List<int> ids = new List<int>();
        for (int i = 0; i < nivelActual.cantidadPares; i++)
        {
            ids.Add(i); ids.Add(i);
        }

        // Barajado simple
        for (int i = 0; i < ids.Count; i++)
        {
            int temp = ids[i];
            int randomIndex = Random.Range(i, ids.Count);
            ids[i] = ids[randomIndex];
            ids[randomIndex] = temp;
        }

        foreach (int id in ids)
        {
            GameObject go = Instantiate(cardPrefab, gridPadre);
            MemoryCard card = go.GetComponent<MemoryCard>();
            card.Inicializar(id, iconos[id], dorso);
            cartas.Add(card);
        }
    }

    IEnumerator SecuenciaInicial()
    {
        gridCanvasGroup.blocksRaycasts = false; // Bloquea clicks

        foreach (var c in cartas) c.Mostrar();

        float tiempoFaltante = nivelActual.tiempoMuestra;
        textCuentaRegresiva.gameObject.SetActive(true);

        while (tiempoFaltante > 0)
        {
            textCuentaRegresiva.text = Mathf.CeilToInt(tiempoFaltante).ToString();
            yield return new WaitForSeconds(1f);
            tiempoFaltante--;
        }
        textCuentaRegresiva.gameObject.SetActive(false);

        foreach (var c in cartas) c.Ocultar();
        gridCanvasGroup.blocksRaycasts = true; // Habilita clicks

        cronometro = nivelActual.tiempoLimite;
        juegoIniciado = true;
    }

    public void SeleccionarCarta(MemoryCard carta)
    {
        carta.Mostrar();
        if (primera == null)
        {
            primera = carta;
        }
        else
        {
            segunda = carta;
            StartCoroutine(CheckMatch());
        }
    }

    IEnumerator CheckMatch()
    {
        gridCanvasGroup.blocksRaycasts = false;

        if (primera.idPar == segunda.idPar)
        {
            paresEncontrados++;
            primera.Desactivar();
            segunda.Desactivar();
            if (paresEncontrados == nivelActual.cantidadPares) Ganar();
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            primera.Ocultar();
            segunda.Ocultar();
        }

        primera = null; segunda = null;
        gridCanvasGroup.blocksRaycasts = true;
    }

    void Update()
    {
        if (!juegoIniciado) return;

        cronometro -= Time.deltaTime;
        textTiempo.text = "Time: " + Mathf.Ceil(cronometro);

        if (cronometro <= 0) Perder();
    }

    void Ganar()
    {
        juegoIniciado = false;

        StopAllCoroutines();

        GameEconomy.AddCoins(nivelActual.recompensa);

        int expGanada = Mathf.RoundToInt(
            nivelActual.cantidadPares * 2f
        );

        StatsPlayer.instance?.AddExp(expGanada);

        panelVictoria.SetActive(true);
    }

    void Perder()
    {
        juegoIniciado = false;
        StopAllCoroutines();

        panelDerrota.SetActive(true);
        Debug.Log("Game Over");
    }

    public void ReintentarNivel()
    {
        panelVictoria.SetActive(false);
        panelDerrota.SetActive(false);

        if (nivelActual != null)
        {
            EmpezarJuego(nivelActual);
        }
    }

    void LimpiarTablero()
    {
        foreach (Transform t in gridPadre) Destroy(t.gameObject);
        cartas.Clear();
        paresEncontrados = 0;
    }

    void AjustarTamañoGrid(MemoryLevel nivel)
    {
        GridLayoutGroup grid = gridPadre.GetComponent<GridLayoutGroup>();
        RectTransform rectPadre = gridPadre.GetComponent<RectTransform>();
        float anchoTotal = rectPadre.rect.width - grid.padding.left - grid.padding.right;
        float espacioTotalHuecos = (nivel.columnasGrid - 1) * grid.spacing.x;
        float tamañoCalculado = (anchoTotal - espacioTotalHuecos) / nivel.columnasGrid;
        grid.cellSize = new Vector2(tamañoCalculado, tamañoCalculado);
    }

    public void RegresarAlMenu()
    {
        juegoIniciado = false;
        StopAllCoroutines();
        LimpiarTablero();

        panelJuego.SetActive(false);
        if (panelVictoria != null) panelVictoria.SetActive(false);
        if (panelDerrota != null) panelDerrota.SetActive(false);

        panelMenu.SetActive(true);

        textTiempo.text = "Time: --";
    }

    public void ExitMiniGame()
    {
        SceneManager.LoadScene("Playground");
    }
}
