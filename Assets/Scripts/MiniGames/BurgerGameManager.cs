using UnityEngine;
using UnityEngine.SceneManagement;

public class BurgerGameManager : MonoBehaviour
{
    [Header("Configuración de Objetos")]
    public GameObject[] ingredientesPrefabs; 

    [Header("Parámetros de Movimiento")]
    public float velocidad = 3.5f;   
    public float limiteX = 2.2f;     

    [Header("Estado del Juego")]
    private GameObject ingredienteActual;
    private bool puedeSoltar = true;

    void Start()
    {
        GenerarIngrediente();
    }

    void Update()
    {
        float x = Mathf.PingPong(Time.time * velocidad, limiteX * 2) - limiteX;
        transform.position = new Vector3(x, transform.position.y, 0);

        if (Input.GetMouseButtonDown(0) && puedeSoltar && ingredienteActual != null)
        {
            Soltar();
        }
    }

    void GenerarIngrediente()
    {
        int indiceAleatorio = Random.Range(0, ingredientesPrefabs.Length);

        ingredienteActual = Instantiate(ingredientesPrefabs[indiceAleatorio], transform.position, Quaternion.identity);

        ingredienteActual.transform.SetParent(transform);

        Rigidbody2D rb = ingredienteActual.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        puedeSoltar = true;
    }

    void Soltar()
    {
        puedeSoltar = false;

        ingredienteActual.transform.SetParent(null);

        Rigidbody2D rb = ingredienteActual.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = true;

        Invoke("GenerarIngrediente", 1.5f);
    }
    public void GameOver()
    {
        Debug.Log("¡La hamburguesa se derrumbó!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
