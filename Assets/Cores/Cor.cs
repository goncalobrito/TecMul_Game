using UnityEngine;

public class GroupColorSync : MonoBehaviour
{
    [Header("Configurações de Luz")]
    public Light targetLight;
    
    [Header("Objetos que vão brilhar")]
    public Renderer[] wallObjects; // Arraste os 4 objetos para cá

    [ColorUsage(true, true)] // Habilita o seletor HDR para brilho intenso
    public Color syncColor = Color.white;

    public float intensidade = 6.4f; // Quanto maior, mais brilha

    void Update()
    {
        // Calculamos a cor final com a intensidade
        Color finalColor = syncColor * intensidade;

        if (targetLight != null) {
            targetLight.color = syncColor;
            targetLight.intensity = intensidade; // Sincroniza a força da luz também
        }

        foreach (Renderer ren in wallObjects)
        {
            if (ren != null)
            {
                // Aplica a cor com intensidade no canal de emissão
                ren.material.SetColor("_EmissionColor", finalColor);
                ren.material.EnableKeyword("_EMISSION");
            }
        }
    }
}