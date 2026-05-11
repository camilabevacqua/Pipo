using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro; 
using UnityEngine.SceneManagement;

public class MinesweeperManager : MonoBehaviour
{
    [Header("Configuración")]
    public int width = 10;
    public int height = 10;
    public int mineCount = 15;

    [Header("Instrucciones")]
    [SerializeField] private GameObject instructionsPanel;

    [Header("Referencias UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI minesCountText;

    [Header("Referencias")]
    public GameObject cellPrefab;
    public Transform gridParent;
    private Cell[,] allCells;

    private float timer;
    private int currentMinesLeft;
    private bool gameStarted;
    private bool gameOver;
    [Header("Paneles de UI")]
    [SerializeField] private GameObject difficultyPanel;
    [SerializeField] private GameObject gameOverPanel;


    void Start()
    {
        currentMinesLeft = mineCount;
        UpdateMinesUI();
        UpdateTimerUI();
        GenerateGrid();
    }

    void Update()
    {
        if (gameStarted && !gameOver)
        {
            timer += Time.deltaTime;
            UpdateTimerUI();
        }
    }
    public void SetDifficultyAndStart(int level)
    {
        GridLayoutGroup grid = gridParent.GetComponent<GridLayoutGroup>();
        RectTransform rect = gridParent.GetComponent<RectTransform>();

        switch (level)
        {
            case 0:
                width = 9; height = 9; mineCount = 10;
                grid.cellSize = new Vector2(65, 65);
                grid.spacing = new Vector2(15, 15);
                grid.padding = new RectOffset(20, 20, 20, 20);
                break;

            case 1:
                width = 12; height = 15; mineCount = 35;
                grid.cellSize = new Vector2(50, 50);
                grid.spacing = new Vector2(10, 10);
                grid.padding = new RectOffset(15, 15, 15, 15);
                break;

            case 2:
                width = 15; height = 20; mineCount = 60;
                grid.cellSize = new Vector2(42, 42);
                grid.spacing = new Vector2(6, 6);
                grid.padding = new RectOffset(10, 10, 10, 10);
                break;
        }

        float totalWidth = (width * grid.cellSize.x) + ((width - 1) * grid.spacing.x) + grid.padding.left + grid.padding.right;
        float totalHeight = (height * grid.cellSize.y) + ((height - 1) * grid.spacing.y) + grid.padding.top + grid.padding.bottom;

        rect.sizeDelta = new Vector2(totalWidth, totalHeight);

        if (difficultyPanel != null) difficultyPanel.SetActive(false);
        RestartGame();
    }

    public void RestartGame()
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        timer = 0;
        currentMinesLeft = mineCount;
        gameStarted = false;
        gameOver = false;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        gridParent.GetComponent<GridLayoutGroup>().constraintCount = width;

        UpdateMinesUI();
        UpdateTimerUI();
        GenerateGrid();
    }
    public void OnBack()
    {
        SceneManager.LoadScene("Playground");
    }
    void GenerateGrid()
    {
        allCells = new Cell[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject obj = Instantiate(cellPrefab, gridParent);
                Cell cell = obj.GetComponent<Cell>();
                cell.Setup(x, y, this);
                allCells[x, y] = cell;
            }
        }

    }

    void PlaceMines()
    {
        int deployedMines = 0;
        while (deployedMines < mineCount)
        {
            int rx = Random.Range(0, width);
            int ry = Random.Range(0, height);

            if (!allCells[rx, ry].isMine)
            {
                allCells[rx, ry].isMine = true;
                deployedMines++;
            }
        }
    }

    void CalculateAdjacentMines()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (allCells[x, y].isMine) continue;

                int count = 0;
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        int nx = x + i;
                        int ny = y + j;

                        if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                        {
                            if (allCells[nx, ny].isMine) count++;
                        }
                    }
                }
                allCells[x, y].SetValue(count);
            }
        }
    }

    public void OnCellClicked(Cell cell)
    {
        if (gameOver) return;

        if (!gameStarted)
        {
            gameStarted = true;
            PlaceMines(cell.x, cell.y);
            CalculateAdjacentMines();
            RevealRecursive(cell.x, cell.y);
            CheckWin();
            return;
        }

        if (cell.isMine)
        {
            gameOver = true;
            RevealAllMines();
            if (gameOverPanel != null) gameOverPanel.SetActive(true);
        }
        else
        {
            RevealRecursive(cell.x, cell.y);
            CheckWin();
        }
    }

    void PlaceMines(int firstX, int firstY)
    {
        int deployedMines = 0;
        while (deployedMines < mineCount)
        {
            int rx = Random.Range(0, width);
            int ry = Random.Range(0, height);

            bool isProtected = false;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (rx == firstX + i && ry == firstY + j)
                    {
                        isProtected = true;
                    }
                }
            }

            if (!isProtected && !allCells[rx, ry].isMine)
            {
                allCells[rx, ry].isMine = true;
                deployedMines++;
            }
        }
    }
    public void ChangeMineCount(int amount)
    {
        if (gameOver) return;
        currentMinesLeft += amount;
        UpdateMinesUI();
    }

    void UpdateMinesUI()
    {
        minesCountText.text = currentMinesLeft.ToString("D3");
    }

    void UpdateTimerUI()
    {
        timerText.text = Mathf.FloorToInt(timer).ToString("D3");
    }

    void RevealRecursive(int x, int y)
    {
        if (x < 0 || y < 0 || x >= width || y >= height) return;

        Cell cell = allCells[x, y];
        if (cell.isRevealed || cell.isMine || cell.isFlagged) return;

        cell.Reveal();

        if (cell.adjacentMines == 0)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    RevealRecursive(x + i, y + j);
                }
            }
        }
    }

    void RevealAllMines()
    {
        foreach (Cell c in allCells)
        {
            if (c.isMine) c.Reveal();
        }
    }
    public void OpenDifficultyMenu()
    {
        gameOver = true;
        gameStarted = false;

        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (difficultyPanel != null) difficultyPanel.SetActive(true);

    }
    public void OpenInstructions()
    {
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(true);
            Time.timeScale = 0f;

            gameOver = true;
        }
    }

    public void CloseInstructions()
    {
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(false);
            Time.timeScale = 1f;

            gameOver = false;
        }
    }
    void CheckWin()
    {
        int hiddenCells = 0;

        foreach (Cell c in allCells)
        {
            if (!c.isRevealed)
            {
                hiddenCells++;
            }
        }

        if (hiddenCells == mineCount)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        gameOver = true;
        gameStarted = false;

        int reward = 5;
        if (width == 12) reward = 10;
        else if (width == 15) reward = 15;

        GameEconomy.AddCoins(reward);

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            gameOverPanel.GetComponentInChildren<TextMeshProUGUI>().text = "YOU WIN!\n+" + reward + " Coins";
        }
    }
}