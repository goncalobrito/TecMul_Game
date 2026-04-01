using UnityEngine;
using System.Collections.Generic;

public class ColorManager : MonoBehaviour
{
    [Header("Configurações de Cor")]
    [ColorUsage(true, true)]
    public Color corAtual = Color.white;
    public float intensidade = 5f;

    private Renderer[] paredesBrilhantes;
    private Light[] luzesLed;
    private List<HiddenObject> itensEscondidos = new List<HiddenObject>();

    public void RegistrarObjeto(HiddenObject obj) => itensEscondidos.Add(obj);

    public Color CorNormalizada => new Color(
        Mathf.Clamp01(corAtual.r),
        Mathf.Clamp01(corAtual.g),
        Mathf.Clamp01(corAtual.b),
        1f
    );

    void Start()
    {
        BuscarLeds();
    }

    void BuscarLeds()
    {
        // Busca tudo com tag Led
        GameObject[] leds = GameObject.FindGameObjectsWithTag("Led");

        List<Renderer> renderers = new List<Renderer>();
        List<Light> luzes = new List<Light>();

        foreach (GameObject led in leds)
        {
            Renderer ren = led.GetComponent<Renderer>();
            if (ren != null) renderers.Add(ren);

            Light luz = led.GetComponent<Light>();
            if (luz != null) luzes.Add(luz);
        }

        paredesBrilhantes = renderers.ToArray();
        luzesLed = luzes.ToArray();

        Debug.Log($"Encontrados {renderers.Count} LEDs e {luzes.Count} luzes.");
    }

    void Update()
    {
        Color corFinal = corAtual * intensidade;

        // Atualiza Renderers (barras LED)
        foreach (Renderer ren in paredesBrilhantes)
        {
            if (ren != null)
            {
                ren.material.SetColor("_EmissionColor", corFinal);
                ren.material.EnableKeyword("_EMISSION");
            }
        }

        // Atualiza Luzes (luz principal e outras com tag Led)
        foreach (Light luz in luzesLed)
        {
            if (luz != null)
            {
                luz.color = corAtual;
                luz.intensity = intensidade;
            }
        }

        foreach (HiddenObject item in itensEscondidos)
            item.ChecarVisibilidade(corAtual);
    }
}