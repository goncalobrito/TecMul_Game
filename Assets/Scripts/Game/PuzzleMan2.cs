using UnityEngine;
using System.Collections;

public class PuzzleMan2 : MonoBehaviour, IInteragivel
{
    public GameObject[] digitos; 
    public Material[] materiaisSimbolos; 

    [Header("Configurações de Câmaras")]
    public Camera cameraPrincipal;
    public Camera camerapuzzle;
    public KeyCode teclaParaSair = KeyCode.Escape;

    [Header("Configuração de Nomes (Hierarquia)")]
    public string nomeDoFilho = "objeto"; // Nome do primeiro filho
    public string nomeDoPlane = "Plane";       // Nome do Plane que tem o material

    public int index = -1; 
    private int[] simbolosIndices; 
    
    public GameObject canvasGeral;
    private bool aVer = false;

    public string TextoInteracao() => "Fazer Puzzle";

    void Awake()
    {
        index = -1;
        simbolosIndices = new int[digitos.Length];
        AtualizarVisualDigitos();
    }

    void Update()
    {
        if (!aVer) return;

        if (Input.GetKeyDown(teclaParaSair)) Sair();

        // Navegação Horizontal (Dígitos)
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            MudarIndiceHorizontal(1);
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            MudarIndiceHorizontal(-1);

        // Navegação Vertical (Materiais no Plane)
        if (index != -1)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                MudarSimboloVertical(1);
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                MudarSimboloVertical(-1);
        }
    }

    void MudarIndiceHorizontal(int direcao)
    {
        index += direcao;
        if (index >= digitos.Length) index = 0;
        if (index < 0) index = digitos.Length - 1;
        AtualizarVisualDigitos();
    }

    void MudarSimboloVertical(int direcao)
    {
        simbolosIndices[index] += direcao;
        if (simbolosIndices[index] >= materiaisSimbolos.Length) simbolosIndices[index] = 0;
        if (simbolosIndices[index] < 0) simbolosIndices[index] = materiaisSimbolos.Length - 1;

        AplicarMaterialNoPlane(index);
    }

    void AplicarMaterialNoPlane(int i)
    {
        // Procura o caminho: Digito -> ObjetoFilho -> Plane
        // O Transform.Find consegue usar barras para entrar na hierarquia
        Transform targetPlane = digitos[i].transform.Find(nomeDoFilho + "/" + nomeDoPlane);

        if (targetPlane != null)
        {
            MeshRenderer renderer = targetPlane.GetComponent<MeshRenderer>();
            if (renderer != null && materiaisSimbolos.Length > 0)
            {
                renderer.material = materiaisSimbolos[simbolosIndices[i]];
            }
        }
        else
        {
            Debug.LogWarning($"Não foi possível encontrar o Plane em: {digitos[i].name}/{nomeDoFilho}/{nomeDoPlane}");
        }
    }

    void AtualizarVisualDigitos()
    {
        for (int i = 0; i < digitos.Length; i++)
        {
            Transform selecionado = digitos[i].transform.Find("Selecionado");
            if (selecionado != null) selecionado.gameObject.SetActive(i == index);
        }
    }

    // Métodos Interagir e Sair mantêm-se iguais...
    public void Interagir()
    {
        aVer = true;
        index = 0;
        GameManager.MenuOcupado = true;
        GameManager.InputBloqueado = true;
        cameraPrincipal.gameObject.SetActive(false);
        camerapuzzle.gameObject.SetActive(true);
        if (canvasGeral != null) canvasGeral.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        AtualizarVisualDigitos();
    }

    public void Sair()
    {
        aVer = false;
        index = -1;
        if (canvasGeral != null) canvasGeral.SetActive(true);
        if (camerapuzzle != null) camerapuzzle.gameObject.SetActive(false);
        cameraPrincipal.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        AtualizarVisualDigitos();
        StartCoroutine(LibertarMenu());
    }

    IEnumerator LibertarMenu()
    {
        yield return new WaitForEndOfFrame();
        GameManager.MenuOcupado = false;
        GameManager.InputBloqueado = false;
    }
}