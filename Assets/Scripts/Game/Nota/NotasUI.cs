using UnityEngine;
using TMPro;

public class NotasUI : MonoBehaviour
{
    public static NotasUI Instance;

    public GameObject painelNota;
    public TextMeshProUGUI textoTitulo;
    public TextMeshProUGUI textoConteudo;

    private bool notaAberta = false;

    void Awake()
    {
        Instance = this;
        painelNota.SetActive(false); // esconde o painel mas o objeto pai fica ativo
    }

    void Update()
    {
        if (notaAberta && Input.GetKeyDown(KeyCode.X))
            FecharNota();
    }

    public void MostrarNota(NotaData nota)
    {
        painelNota.SetActive(true);
        textoTitulo.text = nota.titulo;
        textoConteudo.text = nota.conteudo;
        notaAberta = true;

        // Liberta o rato para ler
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void FecharNota()
    {
        GameManager.InputBloqueado = false;
        painelNota.SetActive(false);
        notaAberta = false;

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}