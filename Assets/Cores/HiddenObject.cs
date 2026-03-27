using UnityEngine;

public class HiddenObject : MonoBehaviour
{
    public Color corDesteObjeto; // Defina no Inspector (ex: Vermelho Puro)
    private Renderer meuRenderer;
    private ColorManager gerenciador;

    void Start()
    {
        meuRenderer = GetComponent<Renderer>();
        // Encontra o gerenciador na cena
        gerenciador = Object.FindFirstObjectByType<ColorManager>();
        
        if (gerenciador != null)
            gerenciador.RegistrarObjeto(this);
    }

    public void ChecarVisibilidade(Color corDaSala)
    {
        // Calcula a distância entre a cor da sala e a cor do objeto
        // Se forem muito parecidas, o objeto "some"
        float diffR = Mathf.Abs(corDaSala.r - corDesteObjeto.r);
        float diffG = Mathf.Abs(corDaSala.g - corDesteObjeto.g);
        float diffB = Mathf.Abs(corDaSala.b - corDesteObjeto.b);

        float diferencaTotal = diffR + diffG + diffB;

        // Se a diferença for menor que 0.1, ele fica invisível
        meuRenderer.enabled = (diferencaTotal > 0.1f);
    }
}