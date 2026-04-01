using UnityEngine;

public class ItemMundo : MonoBehaviour
{
    public ItemData dados;

    void Start()
    {
        // Garante que tem Rigidbody para cair no chão
        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }

    public void Apanhar()
    {
        if (Inventario.Instance.AdicionarItem(dados))
            Destroy(gameObject);
    }
}