using UnityEngine;

public class HiddenObject : MonoBehaviour
{
    public Color corDesteObjeto; // Defina no Inspector (ex: Vermelho Puro)
    private Renderer meuRenderer;
    private ColorManager gerenciador;

    void Start()
    {
        meuRenderer = GetComponent<Renderer>();
        meuRenderer.enabled = false;
        // Encontra o gerenciador na cena
        gerenciador = Object.FindFirstObjectByType<ColorManager>();

        if (gerenciador != null)
            gerenciador.RegistrarObjeto(this);
    }

    public void ChecarVisibilidade(Color corDaSala)
    {
        // Calcula a diferença absoluta entre cada canal de cor (RGB)
        float diffR = Mathf.Abs(corDaSala.r - corDesteObjeto.r);
        float diffG = Mathf.Abs(corDaSala.g - corDesteObjeto.g);
        float diffB = Mathf.Abs(corDaSala.b - corDesteObjeto.b);

        float diferencaTotal = diffR + diffG + diffB;

        // INVERSÃO DA LÓGICA:
        // Se a diferença for PEQUENA (menor que 0.1), o objeto APARECE (true)
        // Se a diferença for GRANDE, o objeto DESAPARECE (false)
        meuRenderer.enabled = (diferencaTotal <= 0.1f);
    }
}