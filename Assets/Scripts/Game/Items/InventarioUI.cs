using UnityEngine;
using UnityEngine.UI;

public class InventarioUI : MonoBehaviour
{
    public GameObject[] slots;          // 5 painéis de slot no Canvas
    public Image[] icones;              // Image dentro de cada slot
    public GameObject[] selecaoVisual; // borda/highlight do slot ativo

    void Start()
    {
        Inventario.Instance.inventarioMudou += AtualizarUI;
        AtualizarUI();
    }

    void Update()
    {
        // Atualiza o slot selecionado visualmente
        for (int i = 0; i < slots.Length; i++)
            selecaoVisual[i].SetActive(i == Inventario.Instance.slotSelecionado);
    }

    void AtualizarUI()
    {
        var itens = Inventario.Instance.itens;
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < itens.Count && itens[i].icone != null)
            {
                icones[i].sprite = itens[i].icone;
                icones[i].enabled = true;
            }
            else
            {
                icones[i].sprite = null;
                icones[i].enabled = false;
            }
        }
    }
}