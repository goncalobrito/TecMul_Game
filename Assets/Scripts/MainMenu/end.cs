using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEnd : MonoBehaviour
{
    // Nome da cena que você quer carregar (ajuste se necessário)
    [SerializeField] private string nomeDaCena = "EndScene";

    // Detecta quando algo entra no Trigger do Collider
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se quem colidiu tem a tag "Player"
        if (other.CompareTag("Player"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(nomeDaCena);
        }
    }

    // Mantive sua função de sair para o botão do menu
    public void Sair()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }
}