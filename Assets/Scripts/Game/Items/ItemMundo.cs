using UnityEngine;

public class ItemMundo : MonoBehaviour
{
    public ItemData dados;

    public void Apanhar()
    {
        if (Inventario.Instance.AdicionarItem(dados))
            Destroy(gameObject);
    }
}