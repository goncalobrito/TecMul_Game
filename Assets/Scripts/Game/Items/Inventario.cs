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

    [Header("Drop")]
    public Transform pontoDropar; // posição à frente do jogador
    public float forcaDrop = 3f;

    public void DroparItemAtual()
    {
        ItemData item = ItemSelecionado();
        if (item == null) return;

        GameObject dropado = Instantiate(
            item.prefabNaMao, // mesmo prefab
            pontoDropar.position,
            Quaternion.identity
        );

        // Reativa física quando dropa
        Rigidbody rb = dropado.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(pontoDropar.forward * forcaDrop, ForceMode.Impulse);
        }

        Collider col = dropado.GetComponent<Collider>();
        if (col != null) col.enabled = true;

        // Adiciona o ItemMundo para poder ser apanhado de novo
        ItemMundo itemMundo = dropado.GetComponent<ItemMundo>();
        if (itemMundo == null) itemMundo = dropado.AddComponent<ItemMundo>();
        itemMundo.dados = item;

        RemoverItem(item);
    }

    public void MostrarItemNaMao(ItemData item)
    {
        if (itemVisualAtual != null) Destroy(itemVisualAtual);
        if (item == null || item.prefabNaMao == null) return;

        itemVisualAtual = Instantiate(item.prefabNaMao, pontoNaMao);

        itemVisualAtual.transform.localPosition = item.posicaoOffset;
        itemVisualAtual.transform.localRotation = Quaternion.Euler(item.rotacaoOffset);

        // Desativa física em todos os Rigidbodies (pai e filhos)
        foreach (Rigidbody rb in itemVisualAtual.GetComponentsInChildren<Rigidbody>())
            rb.isKinematic = true;

        // Desativa todos os Colliders (pai e filhos)
        foreach (Collider col in itemVisualAtual.GetComponentsInChildren<Collider>())
            col.enabled = false;
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

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
        MostrarItemNaMao(ItemSelecionado()); // 👈 adiciona esta linha
    }

    public ItemData ItemSelecionado()
    {
        if (itens.Count == 0 || slotSelecionado >= itens.Count) return null;
        Debug.Log($"Item selecionado: {itens[slotSelecionado]?.nomeItem}");
        return itens[slotSelecionado];
    }
}