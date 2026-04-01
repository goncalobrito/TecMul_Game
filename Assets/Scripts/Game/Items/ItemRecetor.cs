using UnityEngine;

public class ItemRecetor : MonoBehaviour
{
    public ItemData itemNecessario;     // arrasta o ScriptableObject aqui
    public PortaMecanismo porta;        // opcional
    public PuzzleManager puzzle;        // opcional

    private bool jaUsado = false;

    public void TentarUsar(ItemData itemAtual)
    {
        if (jaUsado) return;

        if (itemAtual == null)
        {
            Debug.Log("Não tens nenhum item selecionado.");
            return;
        }

        if (itemAtual == itemNecessario)
        {
            Debug.Log($"Usaste {itemAtual.nomeItem}!");
            jaUsado = true;
            Inventario.Instance.RemoverItem(itemAtual);

            if (porta != null) porta.AbrirFechar();
            if (puzzle != null) puzzle.resolverPuzzle();
        }
        else
        {
            Debug.Log($"Este item não serve aqui.");
        }
    }
}
