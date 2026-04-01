using UnityEngine;

[CreateAssetMenu(fileName = "NovoItem", menuName = "Inventario/Item")]
public class ItemData : ScriptableObject
{
    public string nomeItem;
    public Sprite icone;
    public enum TipoItem { Chave, Nota, Objeto }
    public TipoItem tipo;
    [TextArea] public string descricao;
    public GameObject prefabNaMao; // único prefab para tudo
}