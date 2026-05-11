using UnityEngine;

public class GameEconomy : MonoBehaviour
{
    using UnityEngine;

public static class GameEconomy
{
    private const string COINS_KEY = "PipoCoins";

¿    public static int GetCoins() => PlayerPrefs.GetInt(COINS_KEY, 0);

    public static void AddCoins(int amount)
    {
        int current = GetCoins();
        PlayerPrefs.SetInt(COINS_KEY, current + amount);
        PlayerPrefs.Save();

        UIManager.instance?.UpdateCoinsDisplay();
    }
}