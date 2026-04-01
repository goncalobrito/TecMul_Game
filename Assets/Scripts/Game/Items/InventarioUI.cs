using UnityEngine;
using UnityEngine.UI;

public class InventarioUI : MonoBehaviour
{
    public GameObject[] slots;
    public Image[] icones;
    public Image[] selecaoVisual; // 👈 muda para Image em vez de GameObject

    private ColorManager colorManager;

    void Start()
    {
        if (Inventario.Instance == null)
        {
            Debug.LogError("Inventario não encontrado na cena!");
            return;
        }
        Inventario.Instance.inventarioMudou += AtualizarUI;
        colorManager = Object.FindFirstObjectByType<ColorManager>();
        AtualizarUI();
    }

    void Update()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (selecaoVisual[i] == null)
            {
                Debug.LogError($"selecaoVisual[{i}] está vazio no Inspector!");
                continue;
            }

            bool selecionado = i == Inventario.Instance.slotSelecionado;
            selecaoVisual[i].gameObject.SetActive(selecionado);

            if (selecionado && colorManager != null)
                selecaoVisual[i].color = colorManager.CorNormalizada;
        }
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