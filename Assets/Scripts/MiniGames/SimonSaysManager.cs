using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
public class SimonSaysManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    public static SimonSaysManager Instance { get; private set; } // Para que otros scripts lo encuentren
    [SerializeField] private GameObject gameOverPanel;

    public event EventHandler OnWinGame;
    public event EventHandler OnLoseGame;

    [Header("Configuración del Juego")]
    [SerializeField] private SimonButton[] buttonVisuals;

    [Header("Valores actuales (se cambian por dificultad)")]
    [SerializeField] private float displayDelay = 0.6f;
    [SerializeField] private int roundsToWin = 5;

    [SerializeField] private float speedMultiplier = 0.95f; // Reduce el delay un 5% cada ronda
    [SerializeField] private float minDelay = 0.2f;

    private List<int> sequence = new List<int>();
    private int playerStep = 0;
    private bool inputEnabled = false;
    [SerializeField] private GameObject difficultyPanel;
    private void Awake()
    {
        Instance = this; // Se asigna a sí mismo al nacer
    }

    private void Start()
    {
        foreach (var visual in buttonVisuals)
        {
            if (visual != null && visual.Button != null)
            {
                visual.Button.onClick.AddListener(() => OnPlayerClick(visual.Index));
            }
        }
    }

    // --- NUEVO: Este método lo llamarás desde tus botones de dificultad ---
    public void SetRounds(int rounds)
    {
        roundsToWin = rounds;
    }

    public void SetSpeed(float delay)
    {
        displayDelay = delay;
    }



    public void StartSimonGame()
    {
        sequence.Clear();
        playerStep = 0;
        UpdateScoreDisplay();
        NextRound();
    }

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
            // Usamos sequence.Count porque representa la ronda actual
            scoreText.text = "Ronda: " + sequence.Count.ToString();
        }
    }

private IEnumerator PlaySequence()
    {
        yield return new WaitForSeconds(0.8f);
        foreach (int index in sequence)
        {
            SimonButton targetVisual = GetVisualByIndex(index);
            if (targetVisual != null)
            {
                yield return StartCoroutine(targetVisual.Flash(displayDelay));
                yield return new WaitForSeconds(displayDelay * 0.4f);
            }
        }
        inputEnabled = true;
    }

    private void OnPlayerClick(int index)
    {
        if (!inputEnabled) return;

        if (index == sequence[playerStep])
        {
            // Correcto: Flasheo rápido del botón que tocó el jugador
            StartCoroutine(GetVisualByIndex(index).Flash(0.25f));
            playerStep++;

            // Si completó toda la secuencia actual de esta ronda
            if (playerStep >= sequence.Count)
            {
                // Bloqueamos el input para que no toque nada mientras se genera la siguiente
                inputEnabled = false;
                NextRound();
            }
        }
        else
        {
            // INCORRECTO: Perdió
            inputEnabled = false;

            // Lanzamos el evento por si otros scripts (como sonidos de Wwise) están escuchando
            OnLoseGame?.Invoke(this, EventArgs.Empty);

            // ACTIVAMOS EL PANEL DE DERROTA (GameOver)
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }

            // El panel de dificultad se queda apagado hasta que toque "Retry"
            if (difficultyPanel != null)
            {
                difficultyPanel.SetActive(false);
            }

            Debug.Log("Pipo perdió en la ronda: " + sequence.Count);
        }
    }

    private SimonButton GetVisualByIndex(int index)
    {
        foreach (var visual in buttonVisuals)
        {
            if (visual.Index == index) return visual;
        }
        return null;
    }
    public void RetryGame()
    {
        gameOverPanel.SetActive(false); // Cerramos el de derrota
        difficultyPanel.SetActive(true); // Abrimos el de dificultad
        sequence.Clear(); // Limpiamos la partida anterior
    }

    // Opción B: Reiniciar la escena completa (o volver a la principal)
    public void BackToGameScene()
    {
        // Esto recarga la escena actual en la que estás
        SceneManager.LoadScene("Playground");

        // Si tenés una escena específica de Pipo, podés usar:
        // SceneManager.LoadScene("NombreDeTuEscena");
    }
}
