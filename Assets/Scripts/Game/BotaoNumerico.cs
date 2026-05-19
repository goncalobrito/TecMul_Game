using UnityEngine;

public class BotaoNumerico : MonoBehaviour, IInteragivel
{
    public string valor; // "0" a "9", "DEL" ou "FECHAR"
    public TecladoNumerico teclado;
    public Color corNormal = Color.white;
    public Color corPressionado = Color.cyan;

    private Renderer meuRenderer;

    void Start()
    {
        meuRenderer = GetComponent<Renderer>();
        meuRenderer.material.color = corNormal;
        meuRenderer.material.SetColor("_EmissionColor", corNormal);
        meuRenderer.material.EnableKeyword("_EMISSION");
    }

    public string TextoInteracao() => valor == "DEL" ? "Apagar" : valor == "FECHAR" ? "Fechar" : $"Inserir {valor}";

    public void Interagir()
    {
        AudioManager.Instance.TocarBotao();
        teclado.PrimirBotao(valor);
        StartCoroutine(AnimarBotao());
    }

    System.Collections.IEnumerator AnimarBotao()
    {
        meuRenderer.material.color = corPressionado;
        meuRenderer.material.SetColor("_EmissionColor", corPressionado * 3f);
        yield return new WaitForSeconds(0.15f);
        meuRenderer.material.color = corNormal;
        meuRenderer.material.SetColor("_EmissionColor", corNormal);
    }
}