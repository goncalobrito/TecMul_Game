using UnityEngine;

// GameManager.cs
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static bool InputBloqueado = false;
    public static bool MenuOcupado = false;

    void Awake() => Instance = this;
}