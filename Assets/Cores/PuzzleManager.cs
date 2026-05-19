using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    [Header("Sequência correta de cores")]
    public Color[] sequenciaCorreta; // define no Inspector com as mesmas cores dos botões

    [Header("Referências")]
    public PortaMecanismo porta; // arrasta a porta aqui

    private Color[] sequenciaJogador;
    private int passoAtual = 0;

    void Awake()
    {
        Instance = this;
        sequenciaJogador = new Color[sequenciaCorreta.Length];
    }

    public void RegistarCor(Color cor)
    {
        sequenciaJogador[passoAtual] = cor;

        if (!ColorIgual(sequenciaJogador[passoAtual], sequenciaCorreta[passoAtual]))
        {
            Debug.Log("Sequência errada! Recomeça.");
            passoAtual = 0;
            return;
        }

        passoAtual++;
        Debug.Log($"Passo {passoAtual} correto!");

        if (passoAtual >= sequenciaCorreta.Length)
        {
            Debug.Log("Puzzle resolvido! Porta a abrir.");
            porta.AbrirFechar();
            passoAtual = 0; // reset por segurança
            ColorManager.Instance.corAtual = Color.white; // reset cor para branco
            AudioManager.Instance.TocarSucesso();
        }
    }

    // Compara cores com uma pequena tolerância (evita bugs de float)
    bool ColorIgual(Color a, Color b)
    {
        return Mathf.Abs(a.r - b.r) < 0.01f &&
               Mathf.Abs(a.g - b.g) < 0.01f &&
               Mathf.Abs(a.b - b.b) < 0.01f;
    }

    public void resolverPuzzle()
    {
        // Método para resolver o puzzle por script, se necessário
        passoAtual = 0;
        for (int i = 0; i < sequenciaCorreta.Length; i++)
            RegistarCor(sequenciaCorreta[i]);
    }
}