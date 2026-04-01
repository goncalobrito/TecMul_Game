using UnityEngine;

[CreateAssetMenu(fileName = "novaNota", menuName = "Inventario/Nota")]
public class NotaData : ScriptableObject
{
    public string titulo;
    [TextArea(3, 10)] public string conteudo;
}