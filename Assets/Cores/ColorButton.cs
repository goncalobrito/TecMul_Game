using UnityEngine;

public class ColorButton : MonoBehaviour, IInteragivel
{
    public Color corDesteBotao;
    public ColorManager gerenciador;

    public string TextoInteracao() => "Ativar";

    void Start()
    {
        GetComponent<Renderer>().material.color = corDesteBotao;
    }

    public void Interagir()
    {
        if (gerenciador != null)
            gerenciador.corAtual = corDesteBotao;

        if (PuzzleManager.Instance != null)
            PuzzleManager.Instance.RegistarCor(corDesteBotao);
    }
}