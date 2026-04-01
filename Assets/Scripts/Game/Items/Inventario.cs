using UnityEngine;
using System.Collections.Generic;

public class Inventario : MonoBehaviour
{
    public static Inventario Instance;

    public List<ItemData> itens = new List<ItemData>();
    public int slotSelecionado = 0;
    public int maxSlots = 7;

    //Hello
    public delegate void OnInventarioMudou();
    public event OnInventarioMudou inventarioMudou;

    [Header("Item na mão")]
    public Transform pontoNaMao; // filho da câmara, posicionado à frente
    private GameObject itemVisualAtual;

    public void MostrarItemNaMao(ItemData item)
    {
        if (itemVisualAtual != null) Destroy(itemVisualAtual);
        if (item == null || item.prefabNaMao == null) return;
        itemVisualAtual = Instantiate(item.prefabNaMao, pontoNaMao);
        itemVisualAtual.transform.localPosition = Vector3.zero;
        itemVisualAtual.transform.localRotation = Quaternion.identity;
    }

    void Awake() => Instance = this;

    void Update()
    {
        int slotAnterior = slotSelecionado;

        // Selecionar slot com scroll ou teclas 1-5
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f) slotSelecionado = (slotSelecionado - 1 + maxSlots) % maxSlots;
        if (scroll < 0f) slotSelecionado = (slotSelecionado + 1) % maxSlots;

        for (int i = 0; i < maxSlots; i++)
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                slotSelecionado = i;
        
        if (slotSelecionado != slotAnterior)
        MostrarItemNaMao(ItemSelecionado());
    }

    public bool AdicionarItem(ItemData item)
    {
        if (itens.Count >= maxSlots) return false;
        itens.Add(item);
        inventarioMudou?.Invoke();
        MostrarItemNaMao(ItemSelecionado()); // 👈 linha nova
        return true;
    }

    public void RemoverItem(ItemData item)
    {
        itens.Remove(item);
        if (slotSelecionado >= itens.Count)
            slotSelecionado = Mathf.Max(0, itens.Count - 1);
        inventarioMudou?.Invoke();
    }

    public ItemData ItemSelecionado()
    {
        if (itens.Count == 0 || slotSelecionado >= itens.Count) return null;
        return itens[slotSelecionado];
    }
}