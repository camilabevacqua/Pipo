using UnityEngine;

[CreateAssetMenu(fileName = "NuevoNivel", menuName = "Pipo/Minijuegos/Memoria")]
public class MemoryLevel : ScriptableObject
{
    public string dificultadNombre;
    public int cantidadPares;
    public float tiempoMuestra;
    public float tiempoLimite;
    public int recompensa;
    public int columnasGrid; 
}
