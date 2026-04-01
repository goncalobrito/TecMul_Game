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

    // Já não precisas do array manual — vai buscar pela tag
    private Renderer[] paredesBrilhantes;
    private List<HiddenObject> itensEscondidos = new List<HiddenObject>();

    public void RegistrarObjeto(HiddenObject obj) => itensEscondidos.Add(obj);

    void Start()
    {
        BuscarLeds();
    }

    void BuscarLeds()
    {
        GameObject[] leds = GameObject.FindGameObjectsWithTag("Led");
        paredesBrilhantes = new Renderer[leds.Length];
        for (int i = 0; i < leds.Length; i++)
            paredesBrilhantes[i] = leds[i].GetComponent<Renderer>();
        
        Debug.Log($"Encontrados {leds.Length} LEDs na cena.");
    }

    void Update()
    {
        Color corFinal = corAtual * intensidade;

        if (luzPrincipal != null)
        {
            luzPrincipal.color = corAtual;
            luzPrincipal.intensity = intensidade;
        }

        foreach (Renderer ren in paredesBrilhantes)
        {
            if (ren != null)
            {
                ren.material.SetColor("_EmissionColor", corFinal);
                ren.material.EnableKeyword("_EMISSION");
            }
        }

        foreach (HiddenObject item in itensEscondidos)
            item.ChecarVisibilidade(corAtual);
    }
}