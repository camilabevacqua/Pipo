using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public int x, y;
    public bool isMine, isRevealed, isFlagged;
    public int adjacentMines;
    public TextMeshProUGUI label;
    public Image iconImage;

    private bool isPointerDown;
    private float pointerDownTimer;
    private float requiredHoldTime = 0.4f;
    private MinesweeperManager manager;

    public void Setup(int x, int y, MinesweeperManager m)
    {
        this.x = x;
        this.y = y;
        this.manager = m;
        isRevealed = false;
        isFlagged = false;
        label.text = "";
    }
    public void SetValue(int count)
    {
        adjacentMines = count;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isRevealed) return;
        isPointerDown = true;
        pointerDownTimer = 0;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isPointerDown) return;
        isPointerDown = false;

        if (pointerDownTimer < requiredHoldTime)
        {
            if (!isFlagged) manager.OnCellClicked(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerDown = false;
    }

    void Update()
    {
        if (isPointerDown && !isRevealed)
        {
            pointerDownTimer += Time.deltaTime;
            if (pointerDownTimer >= requiredHoldTime)
            {
                VibratePhone();
                ToggleFlag();
                isPointerDown = false;
            }
        }
    }

    void ToggleFlag()
    {
        isFlagged = !isFlagged;
        label.text = isFlagged ? "🚩" : "";
        label.color = Color.black;
    }

    void VibratePhone()
    {
#if UNITY_ANDROID || UNITY_IOS
        Handheld.Vibrate();
#endif
    }

    public void Reveal()
    {
        if (isRevealed) return;
        isRevealed = true;
        GetComponent<Button>().enabled = false;
        iconImage.color = new Color(0.85f, 0.85f, 0.85f);

        if (isMine)
        {
            label.text = "💣";
            iconImage.color = Color.red;
        }
        else
        {
            label.text = adjacentMines > 0 ? adjacentMines.ToString() : "";
            if (adjacentMines > 0) label.color = GetColorForNumber(adjacentMines);
        }
    }

    private Color GetColorForNumber(int number)
    {
        switch (number)
        {
            case 1: return Color.blue;                          
            case 2: return new Color(0f, 0.5f, 0f);           
            case 3: return Color.red;                          
            case 4: return new Color(0.1f, 0.1f, 0.5f);        
            case 5: return new Color(0.5f, 0f, 0f);         
            case 6: return Color.cyan;                          
            case 7: return Color.black;                       
            case 8: return Color.gray;                          
            default: return Color.black;                        
        }
    }
}