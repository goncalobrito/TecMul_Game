using UnityEngine;
using TMPro;

public class TecladoNumerico : MonoBehaviour, IInteragivel
{
    [Header("Configuração")]
    public string codigoCorreto = "1234";
    public PortaMecanismo porta;

    [Header("Display 3D")]
    public TextMeshPro textoDisplay; // TextMeshPro normal, não UGUI
    public Renderer displayRenderer;
    public Color corErro = Color.red;
    public Color corSucesso = Color.green;
    public Color corNormal = Color.white;

    private string codigoAtual = "";
    private bool resolvido = false;

    public string TextoInteracao() => resolvido ? "" : "Ver teclado";

    public void Interagir() { } // o teclado em si não faz nada, os botões é que interagem

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
                codigoAtual += valor;

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
            if (porta != null) porta.AbrirFechar();
            Debug.Log("Código correto!");
        }
        else
        {
            AudioManager.Instance.TocarErro();
            Debug.Log("Código errado!");
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
            displayRenderer.material.SetColor("_EmissionColor", cor * 2f);
            displayRenderer.material.EnableKeyword("_EMISSION");
        }
    }
}
