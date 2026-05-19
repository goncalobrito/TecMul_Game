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
        painelNota.SetActive(false);
    }

    void Update()
    {
        if (notaAberta && Input.GetKeyDown(KeyCode.Escape))
            FecharNota();
    }

    public void MostrarNota(NotaData nota)
    {
        painelNota.SetActive(true);
        textoTitulo.text = nota.titulo;
        textoConteudo.text = nota.conteudo;
        notaAberta = true;

        GameManager.InputBloqueado = true;
        GameManager.MenuOcupado = true;

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void FecharNota()
    {
        notaAberta = false;

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine(FecharELibertarMenu());
    }

    System.Collections.IEnumerator FecharELibertarMenu()
    {
        yield return new WaitForEndOfFrame();
        painelNota.SetActive(false);
        GameManager.InputBloqueado = false;
        GameManager.MenuOcupado = false;
    }
}