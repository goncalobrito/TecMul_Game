using UnityEngine;

public class ItemMundo : MonoBehaviour, IInteragivel
{
    public ItemData dados;

    void Start()
    {
        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }

    public string TextoInteracao() => dados != null ? $"Apanhar {dados.nomeItem}" : "Apanhar";

    public void Interagir() => Apanhar();

    public void Apanhar()
    {
        AudioManager.Instance.TocarApanharItem();
        Debug.Log($"A apanhar: {dados?.nomeItem} | dados é null: {dados == null}");
        if (Inventario.Instance.AdicionarItem(dados))
            Destroy(gameObject);
    }
}