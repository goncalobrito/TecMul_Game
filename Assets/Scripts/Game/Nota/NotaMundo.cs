using UnityEngine;

public class NotaMundo : MonoBehaviour, IInteragivel
{
    public NotaData dados;

    public string TextoInteracao() => "Ler nota";

    public void Interagir()
    {
        GameManager.InputBloqueado = true;
        NotasUI.Instance.MostrarNota(dados);
    }
}