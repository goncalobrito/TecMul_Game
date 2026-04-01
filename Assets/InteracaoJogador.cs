using UnityEngine;
using TMPro;

public class InteracaoJogador : MonoBehaviour
{
    public float distancia = 3f;
    public GameObject textoUI;
    public Camera cameraPrincipal;

    void Update()
    {
        if (Time.timeScale == 0f) { textoUI.SetActive(false); return; }

        // Drop com Q
        if (Input.GetKeyDown(KeyCode.Q))
            Inventario.Instance.DroparItemAtual();

        Ray raio = cameraPrincipal.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(raio, out hit, distancia))
        {
            // Verifica se o objeto tem uma das tags de interesse
            bool eInteragivel = hit.collider.CompareTag("Item") ||
                               hit.collider.CompareTag("Porta") ||
                               hit.collider.CompareTag("Interactable") ||
                               hit.collider.CompareTag("Nota");

            if (eInteragivel)
            {
                textoUI.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    // LÓGICA DO ITEM
                    if (hit.collider.CompareTag("Item"))
                    {
                        ItemMundo itemMundo = hit.collider.GetComponent<ItemMundo>();
                        if (itemMundo != null)
                            itemMundo.Apanhar();
                        else
                            Destroy(hit.collider.gameObject); // fallback para itens sem ItemData
                    }

                    // USAR ITEM NO INTERACTABLE
                    if (hit.collider.CompareTag("Interactable"))
                    {
                        // Botões de cor
                        ColorButton botao = hit.collider.GetComponent<ColorButton>();
                        if (botao != null)
                            botao.AtivarBotao();

                        // Tentar usar item selecionado
                        ItemRecetor recetor = hit.collider.GetComponent<ItemRecetor>();
                        if (recetor != null)
                            recetor.TentarUsar(Inventario.Instance.ItemSelecionado());
                    }

                    // PORTA COM CHAVE
                    if (hit.collider.CompareTag("Porta"))
                    {
                        var porta = hit.collider.GetComponentInParent<PortaMecanismo>();
                        if (porta != null)
                        {
                            ItemRecetor recetor = hit.collider.GetComponentInParent<ItemRecetor>();
                            if (recetor != null)
                                recetor.TentarUsar(Inventario.Instance.ItemSelecionado());
                            else
                                porta.AbrirFechar(); // porta sem tranca
                        }
                    }

                    if (hit.collider.CompareTag("Nota"))
                    {
                        NotaMundo nota = hit.collider.GetComponent<NotaMundo>();
                        if (nota != null) nota.Ler();
                    }
                }
            }
            else { textoUI.SetActive(false); }
        }
        else { textoUI.SetActive(false); }
    }
}