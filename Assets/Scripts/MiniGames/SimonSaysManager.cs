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

    [SerializeField] private float speedMultiplier = 0.95f; 
    [SerializeField] private float minDelay = 0.2f;

    private List<int> sequence = new List<int>();
    private int playerStep = 0;
    private bool inputEnabled = false;
    [SerializeField] private GameObject difficultyPanel;
    private void Awake()
    {
        Instance = this; 
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
            StartCoroutine(GetVisualByIndex(index).Flash(0.25f));
            playerStep++;

            if (playerStep >= sequence.Count)
            {
                inputEnabled = false;
                NextRound();
            }
        }
        else
        {
            inputEnabled = false;

            OnLoseGame?.Invoke(this, EventArgs.Empty);

            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }

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
        gameOverPanel.SetActive(false); 
        difficultyPanel.SetActive(true); 
        sequence.Clear(); 
    }

    public void BackToGameScene()
    {
        SceneManager.LoadScene("Playground");

        
    }
}
