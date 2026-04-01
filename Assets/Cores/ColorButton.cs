using UnityEngine;

public class ColorButton : MonoBehaviour
{
    public Color corDesteBotao; // Define a cor no Inspector
    public ColorManager gerenciador;

    void Start()
    {
        
        GetComponent<Renderer>().material.color = corDesteBotao;
    }

  
    public void AtivarBotao()
{
    if (gerenciador != null)
    {
        gerenciador.corAtual = corDesteBotao;
        Debug.Log("Sala mudou para a cor: " + corDesteBotao);
    }


    if (PuzzleManager.Instance != null)
        PuzzleManager.Instance.RegistarCor(corDesteBotao);
}
}