using UnityEngine;

public class BotaoSimples : MonoBehaviour, IInteragivel
{
    [Header("Alvo")]
    public MonoBehaviour objetoAbrivel; // Arraste o Elevador aqui

    [Header("Visual")]
    public string mensagem = "Acionar Elevador";
    public MeshRenderer luzBotao;
    public Color corAtivado = Color.green;

    private bool clicado = false;

    public string TextoInteracao() => clicado ? "" : mensagem;

    public void Interagir()
    {
        if (clicado || objetoAbrivel == null) return;

        clicado = true;

        // Verifica se o alvo é um IAbrivel e chama a função
        if (objetoAbrivel is IAbrivel abrivel)
        {
            abrivel.AbrirFechar();
        }

        // Feedback Visual
        if (luzBotao != null)
        {
            luzBotao.material.SetColor("_BaseColor", corAtivado);
            luzBotao.material.SetColor("_EmissionColor", corAtivado * 2f);
            luzBotao.material.EnableKeyword("_EMISSION");
        }

        if (AudioManager.Instance != null) AudioManager.Instance.TocarBotao();
    }
}