using UnityEngine;

public class Lanterna : MonoBehaviour
{
    [Header("Configurações")]
    public Light luzLanterna; 
    public float intensidadeLigada = 50f;
    public KeyCode teclaLanterna = KeyCode.F;

    [Header("O Cilindro da Lâmpada")]
    public MeshRenderer cilindroDaLuz;
    private Material materialDoCilindro;

    [ColorUsage(true, true)]
    public Color corLigada = Color.white;
    public Color corDesligada = Color.black;

    private bool estaLigada = false;

    void Start()
    {
        // Setup inicial da luz
        if (luzLanterna != null)
        {
            luzLanterna.intensity = intensidadeLigada; // Deixa a intensidade pronta
            luzLanterna.enabled = false;
        }

        // Setup do material
        if (cilindroDaLuz != null)
        {
            materialDoCilindro = cilindroDaLuz.material;
            AtualizarVisual(false);
        }
    }

    void Update()
    {
        // 1. Verifica se o inventário existe
        if (Inventario.Instance == null) return;

        // 2. Verifica se a lanterna está na lista de itens do inventário
        bool temLanterna = false;
        for (int i = 0; i < Inventario.Instance.itens.Count; i++)
        {
            // Usando "nomeItem" que é o que está no teu Debug.Log do Inventário
            if (Inventario.Instance.itens[i].nomeItem == "Lanterna UV")
            {
                temLanterna = true;
                break;
            }
        }

        if (!temLanterna) 
        {
            // Se o jogador dropar a lanterna, ela deve desligar
            if (estaLigada) DesligarForçado();
            return;
        }

        // 3. Input
        if (Input.GetKeyDown(teclaLanterna))
        {
            AlternarLanterna();
        }
    }

    void AlternarLanterna()
    {
        estaLigada = !estaLigada;
        
        // Toca o som (se o AudioManager existir)
        if (AudioManager.Instance != null) 
            AudioManager.Instance.TocarLanterna();

        luzLanterna.enabled = estaLigada;
        AtualizarVisual(estaLigada);
    }

    void AtualizarVisual(bool ligada)
    {
        if (materialDoCilindro != null)
        {
            Color corAlvo = ligada ? corLigada : corDesligada;
            
            // Ativa o Emission e muda a cor (funciona em URP e Standard)
            materialDoCilindro.EnableKeyword("_EMISSION");
            materialDoCilindro.SetColor("_EmissionColor", corAlvo);
            materialDoCilindro.SetColor("_BaseColor", corAlvo); // Para URP
            materialDoCilindro.SetColor("_Color", corAlvo);     // Para Standard
        }
    }

    void DesligarForçado()
    {
        estaLigada = false;
        luzLanterna.enabled = false;
        AtualizarVisual(false);
    }
}