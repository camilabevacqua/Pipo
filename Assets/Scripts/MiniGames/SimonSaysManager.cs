using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

public class SimonSaysManager : MonoBehaviour
{// Eventos que escucha tu SimonSaysManagerUI existente
    public event EventHandler OnWinGame;
    public event EventHandler OnLoseGame;

    [Header("Configuración del Juego")]
    // Arrastra aquí tus 4 objetos que tienen el script SimonButtonVisuals
    [SerializeField] private SimonButton[] buttonVisuals;

    // Estos valores podrías cambiarlos externamente según la dificultad
    [SerializeField] private float displayDelay = 0.6f; // Tiempo entre flasheos
    [SerializeField] private int roundsToWin = 5; // Cuántas secuencias hay que adivinar para ganar

    // Lógica interna
    private List<int> sequence = new List<int>();
    private int playerStep = 0;
    private bool inputEnabled = false;

    private void Start()
    {
        // Vinculamos dinámicamente el clic de cada botón a nuestra función de validación
        foreach (var visual in buttonVisuals)
        {
            if (visual != null && visual.Button != null)
            {
                visual.Button.onClick.AddListener(() => OnPlayerClick(visual.Index));
            }
        }
    }

    private void OnDestroy()
    {
        // Buena práctica: desvincular los listeners al destruir el objeto
        foreach (var visual in buttonVisuals)
        {
            if (visual != null && visual.Button != null)
            {
                visual.Button.onClick.RemoveAllListeners();
            }
        }
    }

    // Este método lo activa el SimonSaysManagerUI al habilitar el área de juego
    private void OnEnable()
    {
        StartGame();
    }

    public void StartGame()
    {
        sequence.Clear();
        playerStep = 0;
        NextRound();
    }

    private void NextRound()
    {
        playerStep = 0;
        inputEnabled = false; // Deshabilitamos input mientras se muestra la secuencia

        // Agrega un nuevo botón aleatorio (0-3) a la secuencia
        sequence.Add(UnityEngine.Random.Range(0, buttonVisuals.Length));

        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        // Pequeña pausa antes de empezar para que el jugador se prepare
        yield return new WaitForSeconds(0.8f);

        foreach (int index in sequence)
        {
            // Buscamos el visual que corresponde al índice en la secuencia
            SimonButton targetVisual = GetVisualByIndex(index);
            if (targetVisual != null)
            {
                yield return StartCoroutine(targetVisual.Flash(displayDelay));
                // Pausa corta entre un botón y el siguiente
                yield return new WaitForSeconds(displayDelay * 0.4f);
            }
        }

        // La secuencia terminó, ahora el jugador puede interactuar
        inputEnabled = true;
    }

    // Función que se ejecuta cuando el jugador presiona un botón
    private void OnPlayerClick(int index)
    {
        if (!inputEnabled) return; // Si no es el turno del jugador, ignorar

        // Verificamos si presionó el botón correcto según la secuencia
        if (index == sequence[playerStep])
        {
            // Correcto! Hacemos un flasheo rápido del botón
            StartCoroutine(GetVisualByIndex(index).Flash(0.25f));
            playerStep++;

            // ¿Completó toda la secuencia actual?
            if (playerStep >= sequence.Count)
            {
                // ¿Llegó a la ronda final necesaria para ganar?
                if (sequence.Count >= roundsToWin)
                {
                    OnWinGame?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    // Siguiente ronda (agrega un botón más)
                    NextRound();
                }
            }
        }
        else
        {
            // Incorrecto! Fin del juego.
            inputEnabled = false;
            OnLoseGame?.Invoke(this, EventArgs.Empty);
        }
    }

    // Función auxiliar para encontrar el SimonButtonVisuals con el índice correcto
    private SimonButton GetVisualByIndex(int index)
    {
        foreach (var visual in buttonVisuals)
        {
            if (visual.Index == index) return visual;
        }
        return null;
    }
}
