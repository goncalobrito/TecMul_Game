using UnityEngine;
using System.Collections.Generic;

public class ColorManager : MonoBehaviour
{
    [Header("Configurações de Cor")]
    [ColorUsage(true, true)] 
    public Color corAtual = Color.white;
    public float intensidade = 5f;

    [Header("Referências da Cena")]
    public Light luzPrincipal;
    public Renderer[] paredesBrilhantes;

    // Lista automática de objetos que se escondem
    private List<HiddenObject> itensEscondidos = new List<HiddenObject>();

    // Função para outros scripts registrarem objetos escondidos aqui
    public void RegistrarObjeto(HiddenObject obj) => itensEscondidos.Add(obj);

    void Update()
    {
        Color corFinal = corAtual * intensidade;

        // 1. Atualiza a Luz
        if (luzPrincipal != null)
        {
            luzPrincipal.color = corAtual;
            luzPrincipal.intensity = intensidade;
        }

        // 2. Atualiza as Paredes
        foreach (Renderer ren in paredesBrilhantes)
        {
            if (ren != null)
            {
                ren.material.SetColor("_EmissionColor", corFinal);
                ren.material.EnableKeyword("_EMISSION");
            }
        }

        // 3. Avisa os objetos escondidos para checarem a camuflagem
        foreach (HiddenObject item in itensEscondidos)
        {
            item.ChecarVisibilidade(corAtual);
        }
    }
}