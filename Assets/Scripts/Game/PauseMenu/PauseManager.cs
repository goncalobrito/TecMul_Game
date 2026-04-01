using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI; // arrasta o painel de pause aqui
    private bool emPausa = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (emPausa) Retomar();
            else Pausar();
        }
    }

    public void Pausar()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;       // congela o jogo
        Cursor.lockState = CursorLockMode.None; // liberta o rato
        Cursor.visible = true;
        emPausa = true;
    }

    public void Retomar()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;       // retoma o jogo
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        emPausa = false;
    }

    public void VoltarMenu()
    {
        Time.timeScale = 1f;       // importante! reset antes de mudar de scene
        SceneManager.LoadScene("MainMenu");
    }
}