using UnityEngine;

public class LogicGatePorta : MonoBehaviour, IAbrivel
{
    [Header("Configurações")]
    public int itensNecessarios = 2;
    private int itensRecebidos = 0;

    [Header("Alvos das Luzes (MeshRenderers)")]
    public MeshRenderer rendererLuz1;
    public MeshRenderer rendererLuz2;

    [Header("Cores")]
    [ColorUsage(true, true)] // Permite selecionar cores HDR (brilhantes) no Inspector
    public Color corDesligada = Color.black;
    
    [ColorUsage(true, true)]
    public Color corLigada = Color.green;

    [Header("Alvo Final")]
    public MonoBehaviour scriptPorta; 

    void Start()
    {
        // Inicializa as luzes no estado desligado
        ResetarLuzes();
    }

    void ResetarLuzes()
    {
        if (rendererLuz1) AplicarEstadoLuz(rendererLuz1, false);
        if (rendererLuz2) AplicarEstadoLuz(rendererLuz2, false);
    }

    public void AbrirFechar()
    {
        VerificarSinal();
    }

    void VerificarSinal()
    {
        itensRecebidos++;

        if (itensRecebidos == 1 && rendererLuz1 != null)
            AplicarEstadoLuz(rendererLuz1, true);
        else if (itensRecebidos >= 2 && rendererLuz2 != null)
            AplicarEstadoLuz(rendererLuz2, true);

        Debug.Log($"Sinal recebido! ({itensRecebidos}/{itensNecessarios})");

        if (itensRecebidos >= itensNecessarios)
        {
            AtivarPorta();
        }
    }

    void AplicarEstadoLuz(MeshRenderer rend, bool ligar)
    {
        // Criamos uma instância do material para não afetar outros objetos que usem o mesmo material
        Material mat = rend.material; 
        Color corAlvo = ligar ? corLigada : corDesligada;

        if (ligar)
        {
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", corAlvo);
            // Caso estejas a usar URP, a linha abaixo ajuda a garantir a cor principal
            mat.color = corAlvo; 
        }
        else
        {
            mat.DisableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", Color.black);
            mat.color = corAlvo;
        }
    }

    void AtivarPorta()
    {
        if (scriptPorta is IAbrivel porta)
        {
            porta.AbrirFechar();
            Debug.Log("Sistema Completo! A abrir porta...");
        }
    }
}