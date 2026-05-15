using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class SimonSaysManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Texto recompensa")]
    [SerializeField] private TextMeshProUGUI coinsRewardText;

    public static SimonSaysManager Instance { get; private set; }

    [SerializeField] private GameObject gameOverPanel;

    public event EventHandler OnLoseGame;

    [Header("Configuración del Juego")]
    [SerializeField] private SimonButton[] buttonVisuals;

    [Header("Velocidad")]
    [SerializeField] private float displayDelay = 0.6f;
    [SerializeField] private float speedMultiplier = 0.95f;
    [SerializeField] private float minDelay = 0.2f;

    [Header("Economía")]
    [SerializeField] private int difficultyMultiplier = 1;

    [Header("UI")]
    [SerializeField] private GameObject difficultyPanel;

    private List<int> sequence = new List<int>();

    private int playerStep = 0;
    private bool inputEnabled = false;

    private void Awake()
    {
        Instance = this;
    }

    // =========================
    // DIFICULTAD
    // =========================

    public void SetDifficulty(int multiplier)
    {
        difficultyMultiplier = multiplier;

        Debug.Log("Dificultad x" + multiplier);
    }

    public void SetSpeed(float delay)
    {
        displayDelay = delay;

        Debug.Log("Velocidad: " + delay);
    }

    // =========================
    // INICIO
    // =========================

    public void StartSimonGame()
    {
        StopAllCoroutines();

        sequence.Clear();

        playerStep = 0;

        inputEnabled = false;

        if (difficultyPanel != null)
        {
            difficultyPanel.SetActive(false);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        UpdateScoreDisplay();

        NextRound();

        Debug.Log("Juego iniciado");
    }

    // =========================
    // RONDAS
    // =========================

    private void NextRound()
    {
        playerStep = 0;

        inputEnabled = false;

        if (displayDelay > minDelay)
        {
            displayDelay *= speedMultiplier;
        }

        sequence.Add(UnityEngine.Random.Range(0, buttonVisuals.Length));

        UpdateScoreDisplay();

        StartCoroutine(PlaySequence());
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Ronda: " + sequence.Count;
        }
    }

    // =========================
    // SECUENCIA
    // =========================

    private IEnumerator PlaySequence()
    {
        yield return new WaitForSeconds(0.8f);

        // COPIA para evitar modificar la lista original
        List<int> currentSequence = new List<int>(sequence);

        foreach (int index in currentSequence)
        {
            SimonButton targetVisual = GetVisualByIndex(index);

            if (targetVisual != null)
            {
                yield return StartCoroutine(targetVisual.Flash(displayDelay));

                yield return new WaitForSeconds(displayDelay * 0.4f);
            }
        }

        inputEnabled = true;

        Debug.Log("Input activado");
    }

    // =========================
    // INPUT
    // =========================

    private void OnPlayerClick(int index)
    {
        if (!inputEnabled) return;

        // Correcto
        if (index == sequence[playerStep])
        {
            StartCoroutine(GetVisualByIndex(index).Flash(0.25f));

            playerStep++;

            // completó ronda
            if (playerStep >= sequence.Count)
            {
                inputEnabled = false;

                NextRound();
            }
        }

        // Perdió
        else
        {
            LoseGame();
        }
    }

    // =========================
    // PERDER
    // =========================

    private void LoseGame()
    {
        inputEnabled = false;

        int rondaCompletada = Mathf.Max(1, sequence.Count - 1);

        int monedasGanadas =
            rondaCompletada * difficultyMultiplier;

        GameEconomy.AddCoins(monedasGanadas);

        if (coinsRewardText != null)
        {
            coinsRewardText.text =
                "Ronda alcanzada: " + rondaCompletada +
                "\nGanaste " + monedasGanadas + " monedas";
        }

        Debug.Log("Perdió en ronda: " + sequence.Count);
        Debug.Log("Monedas: " + monedasGanadas);

        OnLoseGame?.Invoke(this, EventArgs.Empty);

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    // =========================
    // HELPERS
    // =========================

    private SimonButton GetVisualByIndex(int index)
    {
        foreach (var visual in buttonVisuals)
        {
            if (visual.Index == index)
            {
                return visual;
            }
        }

        return null;
    }

    // =========================
    // UI
    // =========================

    public void RetryGame()
    {
        StopAllCoroutines();

        sequence.Clear();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (difficultyPanel != null)
        {
            difficultyPanel.SetActive(true);
        }
    }

    public void BackToGameScene()
    {
        SceneManager.LoadScene("Playground");
    }

    // =========================
    // BOTONES SIMON
    // =========================

    private void Start()
    {
        foreach (var visual in buttonVisuals)
        {
            if (visual != null && visual.Button != null)
            {
                visual.Button.onClick.AddListener(() =>
                    OnPlayerClick(visual.Index));
            }
        }
    }
}