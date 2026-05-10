using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MinesweeperManager : MonoBehaviour
{
    [Header("Configuración")]
    public int width = 10;
    public int height = 10;
    public int mineCount = 15;

    [Header("Referencias")]
    public GameObject cellPrefab;
    public Transform gridParent; 
    private Cell[,] allCells;

    void Start()
    {
        GenerateGrid();
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
                cell.GetComponent<Button>().onClick.AddListener(() => OnCellClicked(cell));

                allCells[x, y] = cell;
            }
        }

        PlaceMines();
        CalculateAdjacentMines();
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
        if (cell.isMine)
        {
            Debug.Log("¡BOOM! Perdiste.");
            RevealAllMines();
        }
        else
        {
            RevealRecursive(cell.x, cell.y);
        }
    }

    void RevealRecursive(int x, int y)
    {
        if (x < 0 || y < 0 || x >= width || y >= height) return;

        Cell cell = allCells[x, y];
        if (cell.isRevealed || cell.isMine) return;

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
}