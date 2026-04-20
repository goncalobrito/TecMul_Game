using UnityEngine;

public class NotaMundo : MonoBehaviour, IInteragivel
{
    public NotaData dados;

    public string TextoInteracao() => "Ler nota";

    public void Interagir()
    {
        NotasUI.Instance.MostrarNota(dados);
    }
}