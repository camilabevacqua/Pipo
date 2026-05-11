using UnityEngine;
using UnityEngine.UI;

public class MemoryCard : MonoBehaviour
{
    public int idPar;
    private Image imagenCarta;
    private Sprite spriteFrente;
    private Sprite spriteDorso;
    private Button boton;

    private void Awake()
    {
        boton = GetComponent<Button>();
        boton.onClick.AddListener(OnClickCard);
    }

    public void Inicializar(int id, Sprite frente, Sprite dorso)
    {
        idPar = id;
        spriteFrente = frente;
        spriteDorso = dorso;
        imagenCarta = GetComponent<Image>();
        Ocultar();
    }

    public void Mostrar()
    {
        imagenCarta.sprite = spriteFrente;
        boton.interactable = false;
    }

    public void Ocultar()
    {
        imagenCarta.sprite = spriteDorso;
        boton.interactable = true;
    }

    public void Desactivar()
    {
        boton.interactable = false;
        //bajar el alpha para indicar que ya se encontró
    }

    public void OnClickCard()
    {
        MemoryGameManager.Instance.SeleccionarCarta(this);
    }
}
