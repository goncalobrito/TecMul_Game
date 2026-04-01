using UnityEngine;

public class ColorButton : MonoBehaviour
{
    public Color corDesteBotao; // Define a cor no Inspector
    private ColorManager gerenciador;

    void Start()
    {
        // Encontra o gerenciador na cena
        gerenciador = Object.FindFirstObjectByType<ColorManager>();
        
        // Opcional: Muda a cor visual do próprio botão para sabermos qual é qual
        GetComponent<Renderer>().material.color = corDesteBotao;
    }

    // Esta função será chamada quando interagirmos com o botão
    public void AtivarBotao()
{
    if (gerenciador != null)
    {
        gerenciador.corAtual = corDesteBotao;
        Debug.Log("Sala mudou para a cor: " + corDesteBotao);
    }

    // 👇 LINHA NOVA — regista a cor no puzzle
    if (PuzzleManager.Instance != null)
        PuzzleManager.Instance.RegistarCor(corDesteBotao);
}
}