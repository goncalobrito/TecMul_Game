using UnityEngine;
using System.Collections.Generic;

public class ItemRecetor : MonoBehaviour, IInteragivel
{
    public ItemData itemNecessario;

    [Header("Aparência Física")]
    [Tooltip("O objeto 3D que deve aparecer quando o item é usado")]
    public GameObject objetoFisicoParaAtivar;

    [Header("Alvos")]
    public List<MonoBehaviour> alvos;

    private bool jaUsado = false;

    public string TextoInteracao() => itemNecessario != null && !jaUsado
        ? $"Usar {itemNecessario.nomeItem}"
        : (jaUsado ? "" : "Interagir"); // Remove o texto se já foi usado

    public void Interagir()
    {
        if (jaUsado) return;

        ItemData itemAtual = Inventario.Instance.ItemSelecionado();
        TentarUsar(itemAtual);
    }

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
            AudioManager.Instance.TocarChave();
            jaUsado = true;

            // 1. Remove do inventário
            Inventario.Instance.RemoverItem(itemAtual);

            // 2. MOSTRA O ITEM FISICAMENTE
            if (objetoFisicoParaAtivar != null)
            {
                objetoFisicoParaAtivar.SetActive(true);
            }

            // 3. Executa as ações nos alvos
            foreach (var alvo in alvos)
            {
                if (alvo is IAbrivel abrivel)
                {
                    abrivel.AbrirFechar();
                }
                else if (alvo is IInteragivel interagivel) // O 'else' impede o segundo sinal
                {
                    interagivel.Interagir();
                }
            }
        }
        else
        {
            Debug.Log("Este item não serve aqui.");
            AudioManager.Instance.TocarErro();
        }
    }
}