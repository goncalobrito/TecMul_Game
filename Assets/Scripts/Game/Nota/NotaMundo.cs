using UnityEngine;

public class NotaMundo : MonoBehaviour
{
    public NotaData dados;

    public void Ler()
    {
        NotasUI.Instance.MostrarNota(dados);
    }
}