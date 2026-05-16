using System.Collections.Generic;
using UnityEngine;

public class CatchPool : MonoBehaviour
{
    public static CatchPool instance;

    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private int cantidadInicial = 20;

    private List<GameObject> pool = new List<GameObject>();

    void Awake()
    {
        instance = this;

        for (int i = 0; i < cantidadInicial; i++)
        {
            CrearObjeto();
        }
    }

    GameObject CrearObjeto()
    {
        int randomIndex = Random.Range(0, prefabs.Length);

        GameObject obj =
            Instantiate(prefabs[randomIndex]);

        obj.SetActive(false);

        pool.Add(obj);

        return obj;
    }

    public GameObject ObtenerObjeto()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        return CrearObjeto();
    }
}