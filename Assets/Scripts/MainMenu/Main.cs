using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void JogarAgora()
    {
        SceneManager.LoadScene("Game"); // nome da tua scene do jogo
    }

    public void Sair()
    {
        Application.Quit();

    }
}