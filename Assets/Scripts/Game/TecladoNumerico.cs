using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TecladoNumerico : MonoBehaviour, IInteragivel
{
    [Header("Configuração")]
    public string codigoCorreto;
    public Camera cameraTeclado; // Arraste a câmera focada aqui

    [Header("Alvos")]
    public List<MonoBehaviour> alvos;

    [Header("Display 3D")]
    public TextMeshPro textoDisplay;
    public Renderer displayRenderer;
    public Color corErro = Color.red;
    public Color corSucesso = Color.green;
    public Color corNormal = Color.white;

    private string codigoAtual = "";
    private bool resolvido = false;
    private bool estaFocado = false;

    void Update()
    {
        if (!estaFocado || resolvido) return;

        // Detetar teclas de 0 a 9 (Alpha e Keypad)
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(i.ToString()) || Input.GetKeyDown("[" + i + "]"))
            {
                PrimirBotao(i.ToString());
            }
        }

        // Backspace para apagar
        if (Input.GetKeyDown(KeyCode.Backspace)) PrimirBotao("DEL");

        // ESC para sair do teclado
        if (Input.GetKeyDown(KeyCode.Escape)) SairDoTeclado();
    }

    public string TextoInteracao() => resolvido ? "" : "Usar Teclado";

    public void Interagir()
    {
        if (resolvido) return;
        EntrarNoTeclado();
    }

    void EntrarNoTeclado()
    {
        estaFocado = true;
        cameraTeclado.gameObject.SetActive(true);
        // Aqui deves desativar o movimento do teu Player
        GameManager.MenuOcupado = true;
        GameManager.InputBloqueado = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void SairDoTeclado()
    {
        estaFocado = false;
        cameraTeclado.gameObject.SetActive(false);
        // Reativar movimento do Player
        GameManager.MenuOcupado = false;
        GameManager.InputBloqueado = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(LibertarMenu());
    }

    System.Collections.IEnumerator LibertarMenu()
    {
        yield return new WaitForEndOfFrame();
        GameManager.MenuOcupado = false;
        GameManager.InputBloqueado = false;
    }

    public void PrimirBotao(string valor)
    {
        if (resolvido) return;

        if (valor == "DEL")
        {
            if (codigoAtual.Length > 0)
                codigoAtual = codigoAtual.Substring(0, codigoAtual.Length - 1);
        }
        else
        {
            if (codigoAtual.Length < codigoCorreto.Length)
            {
                codigoAtual += valor;
                AudioManager.Instance.TocarBotao(); // Tocar som ao digitar
            }

            if (codigoAtual.Length == codigoCorreto.Length)
            {
                VerificarCodigo();
                return;
            }
        }
        AtualizarDisplay(corNormal);
    }

    void VerificarCodigo()
    {
        if (codigoAtual == codigoCorreto)
        {
            AudioManager.Instance.TocarSucesso();
            resolvido = true;
            AtualizarDisplay(corSucesso);
            foreach (var alvo in alvos)
            {
                if (alvo is IAbrivel abrivel) abrivel.AbrirFechar();
            }
            Invoke("SairDoTeclado", 1.0f); // Sai automaticamente após sucesso
        }
        else
        {
            AudioManager.Instance.TocarErro();
            StartCoroutine(ResetarDisplay());
        }
    }

    System.Collections.IEnumerator ResetarDisplay()
    {
        AtualizarDisplay(corErro);
        yield return new WaitForSeconds(0.5f);
        codigoAtual = "";
        AtualizarDisplay(corNormal);
    }

    void AtualizarDisplay(Color cor)
    {
        if (textoDisplay != null)
        {
            textoDisplay.text = codigoAtual.PadLeft(codigoCorreto.Length, '_');
            textoDisplay.color = cor;
        }

        if (displayRenderer != null)
        {
            displayRenderer.material.SetColor("_EmissionColor", cor * 1f);
            displayRenderer.material.EnableKeyword("_EMISSION");
        }
    }
}