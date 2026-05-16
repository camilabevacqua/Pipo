using System.Collections.Generic;
using UnityEngine;

public class CatchPool : MonoBehaviour
{
    public static CatchPool instance;

    private List<GameObject> pool = new List<GameObject>();

    void Awake()
    {
        instance = this;
    }

    public GameObject ObtenerObjeto(GameObject prefab)
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy &&
                obj.name.Contains(prefab.name))
            {
                return obj;
            }
        }

        GameObject nuevo =
            Instantiate(prefab);

        nuevo.name = prefab.name;

        nuevo.SetActive(false);

        pool.Add(nuevo);

        return nuevo;
    }
}