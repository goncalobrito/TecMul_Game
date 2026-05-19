using UnityEngine;

public class ShotgunController : MonoBehaviour
{
    [Header("Configurações de Cadência")]
    public int mouseButton = 0; 
    public float intervaloEntreCanos = 0.2f; // Tempo rápido entre o tiro 1 e 2
    public float tempoRecargaCano = 1.0f;    // Tempo longo após os 2 tiros
    
    private float proximoTiroDisponivel = 0f;
    private int tirosDados = 0; // Contador de tiros

    void Update()
    {
        if (GameManager.InputBloqueado || GameManager.MenuOcupado) return;
        if (!TemCacadeiraNoInventario()) return;

        if (Input.GetMouseButtonDown(mouseButton) && Time.time >= proximoTiroDisponivel)
        {
            Atirar();
        }
    }

    void Atirar()
    {
        // Tocar som
        if (AudioManager.Instance != null)
            AudioManager.Instance.TocarCaçadeira();

        tirosDados++; // Incrementa o contador

        if (tirosDados < 2)
        {
            // Se ainda só deu o 1º tiro, o próximo pode ser rápido
            proximoTiroDisponivel = Time.time + intervaloEntreCanos;
            Debug.Log("PUM! Primeiro cano.");
        }
        else
        {
            // Se deu o 2º tiro, agora tem de esperar o tempo de recarga
            proximoTiroDisponivel = Time.time + tempoRecargaCano;
            tirosDados = 0; // Reseta o contador para o próximo ciclo
            Debug.Log("PUM! Segundo cano. A recarregar...");
        }
    }

    bool TemCacadeiraNoInventario()
    {
        if (Inventario.Instance == null) return false;
        foreach (var item in Inventario.Instance.itens)
        {
            if (item.nomeItem == "Espingarda") return true;
        }
        return false;
    }
}