
    using UnityEngine;

public class PortalSaida : MonoBehaviour
{
    [Header("UI")]
    public GameObject painelEscapaste;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entrou no trigger: " + other.gameObject.name); 
        if (other.CompareTag("Player"))
        {
            painelEscapaste.SetActive(true);
            Time.timeScale = 0f; // pausa o jogo
        }
    }
}

