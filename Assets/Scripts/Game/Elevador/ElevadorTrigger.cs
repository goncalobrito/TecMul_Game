using UnityEngine;

public class ElevadorTrigger : MonoBehaviour
{
    // Arraste o objeto Pai (que tem o ElevadorController) para aqui no Inspector
    public ElevadorController controller;

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player saiu do elevador trigger: " + other.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("A ativar o fecho das portas do elevador pela saída.");
            controller.AtivarFechoPelaSaida();
        }
    }
}