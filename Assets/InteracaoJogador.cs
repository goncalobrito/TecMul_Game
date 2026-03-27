using UnityEngine;
using TMPro;

public class InteracaoJogador : MonoBehaviour
{
    public float distancia = 3f;
    public GameObject textoUI;
    public Camera cameraPrincipal; // Arrasta a câmera aqui no Inspector

    void Update()
    {
        // Usa a câmera diretamente em vez de transform.forward
        Ray raio = cameraPrincipal.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(raio, out hit, distancia))
        {
            if (hit.collider.CompareTag("Item") || hit.collider.CompareTag("Porta"))
            {
                textoUI.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (hit.collider.CompareTag("Item"))
                    {
                        Destroy(hit.collider.gameObject);
                        Debug.Log("Apanhaste o item!");
                    }

                    if (hit.collider.CompareTag("Porta"))
                    {
                        hit.collider.GetComponentInParent<PortaMecanismo>().AbrirFechar();
                    }
                }
            }
            else { textoUI.SetActive(false); }
        }
        else { textoUI.SetActive(false); }
    }
}