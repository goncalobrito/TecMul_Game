using UnityEngine;

public class InteracaoJogador : MonoBehaviour
{
    public float distancia = 3f;
    public GameObject textoUI;
    public Camera cameraPrincipal;

    void Update()
    {
        if (GameManager.InputBloqueado) return;

        if (Time.timeScale == 0f) { textoUI.SetActive(false); return; }

        if (Input.GetKeyDown(KeyCode.Q))
            Inventario.Instance.DroparItemAtual();

        Ray raio = cameraPrincipal.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(raio, out hit, distancia))
        {
            IInteragivel interagivel = hit.collider.GetComponent<IInteragivel>();

            if (interagivel != null)
            {
                textoUI.SetActive(true);
                textoUI.GetComponent<TMPro.TextMeshProUGUI>().text = interagivel.TextoInteracao();

                if (Input.GetKeyDown(KeyCode.E))
                    interagivel.Interagir();
            }
            else
            {
                textoUI.SetActive(false);
            }
        }
        else
        {
            textoUI.SetActive(false);
        }
    }
}