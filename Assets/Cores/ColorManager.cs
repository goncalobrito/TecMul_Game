using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class ColorManager : MonoBehaviour
{
    [Header("Configurações de Cor")]
    public bool modoDiscoAtivo = false;
    [ColorUsage(true, true)]
    public Color corAtual = Color.white;
    public float intensidade = 5f;

    public static ColorManager Instance;

    private Renderer[] paredesBrilhantes;
    private Light[] luzesLed;
    private List<HiddenObject> itensEscondidos = new List<HiddenObject>();

    // Cache do ID da propriedade para performance
    private static readonly int EmissionColorID = Shader.PropertyToID("_EmissionColor");

    public void RegistrarObjeto(HiddenObject obj) => itensEscondidos.Add(obj);

    public Color CorNormalizada => new Color(
        Mathf.Clamp01(corAtual.r),
        Mathf.Clamp01(corAtual.g),
        Mathf.Clamp01(corAtual.b),
        1f
    );

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        BuscarLeds();

        // Se o modo disco começar ativo, inicia a coroutine
        if (modoDiscoAtivo)
        {
            StartCoroutine(RotinaDisco());
        }
        corAtual = Color.white; 
    }

    void BuscarLeds()
    {
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

        Debug.Log($"ColorManager: Encontrados {renderers.Count} LEDs e {luzes.Count} luzes.");
    }

    // Ligue este método ao evento OnClick() do seu botão na UI
    public void AlternarDisco()
    {
        modoDiscoAtivo = !modoDiscoAtivo;

        if (modoDiscoAtivo)
        {
            // Usamos o nome da função como string para garantir que possamos pará-la especificamente
            StartCoroutine("RotinaDisco");
        }
        else
        {
            StopCoroutine("RotinaDisco");
            // Opcional: Voltar para uma cor padrão (ex: branco) ao desligar
            corAtual = Color.white; 
        }
    }

    private IEnumerator RotinaDisco()
    {
        float intervalo = 0.2f; // Velocidade da troca de cores
        Color[] cores = { Color.red, Color.green, Color.blue, Color.magenta, Color.yellow };
        int indiceAtual = 0;

        while (modoDiscoAtivo)
        {
            corAtual = cores[indiceAtual];
            indiceAtual = (indiceAtual + 1) % cores.Length;
            yield return new WaitForSeconds(intervalo);
        }
    }

    void Update()
    {

        Color corFinal = corAtual * intensidade;

        // Atualiza Renderers
        foreach (Renderer ren in paredesBrilhantes)
        {
            if (ren != null)
            {
                // Usar o ID da propriedade é mais rápido que usar a String
                ren.material.SetColor(EmissionColorID, corFinal);
                ren.material.EnableKeyword("_EMISSION");
            }
        }

        // Atualiza Luzes
        foreach (Light luz in luzesLed)
        {
            if (luz != null)
            {
                luz.color = corAtual;
                luz.intensity = intensidade;
            }
        }

        // Atualiza itens escondidos
        for (int i = itensEscondidos.Count - 1; i >= 0; i--)
        {
            if (itensEscondidos[i] != null)
                itensEscondidos[i].ChecarVisibilidade(corAtual);
        }
    }
}