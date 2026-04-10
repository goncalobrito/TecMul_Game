using UnityEngine;

public class ItemRecetor : MonoBehaviour, IInteragivel
{
    public ItemData itemNecessario;
    public PortaMecanismo porta;
    public PuzzleManager puzzle;

    private bool jaUsado = false;

    public string TextoInteracao() => itemNecessario != null 
        ? $"Usar {itemNecessario.nomeItem}" 
        : "Interagir";

    public void Interagir()
    {
        ItemData itemAtual = Inventario.Instance.ItemSelecionado();
        Debug.Log($"Tentativa de usar '{itemAtual?.nomeItem}' no recetor de '{itemNecessario?.nomeItem}'");
        TentarUsar(itemAtual);
    }

    public void TentarUsar(ItemData itemAtual)
    {
        Debug.Log($"itemAtual: {itemAtual?.nomeItem} | ID: {itemAtual?.GetInstanceID()}");
        Debug.Log($"itemNecessario: {itemNecessario?.nomeItem} | ID: {itemNecessario?.GetInstanceID()}");
        if (jaUsado) return;

        if (itemAtual == null)
        {
            Debug.Log("Não tens nenhum item selecionado.");
            return;
        }

        if (itemAtual == itemNecessario)
        {
            AudioManager.Instance.TocarChave();
            Debug.Log($"Usaste {itemAtual.nomeItem}!");
            jaUsado = true;
            Inventario.Instance.RemoverItem(itemAtual);

            if (porta != null) porta.AbrirFechar();
            if (puzzle != null) puzzle.resolverPuzzle();
        }
        else
        {
            Debug.Log("Este item não serve aqui.");
            AudioManager.Instance.TocarErro();
        }
    }
}