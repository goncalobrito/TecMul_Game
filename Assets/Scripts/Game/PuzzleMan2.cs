using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleMan2 : MonoBehaviour, IInteragivel
{
    public GameObject[] digitos; 
    public Material[] materiaisSimbolos; 

    [Header("Configuração da Solução")]
    public int[] solucaoCorreta; 
    public GameObject objetoParaAbrir; 
    public KeyCode teclaConfirmar = KeyCode.Space;

    public GameObject texto; 

    [Header("Configurações de Câmaras")]
    public Camera cameraPrincipal;
    public Camera camerapuzzle;
    public KeyCode teclaParaSair = KeyCode.Escape;

    [Header("Nomes na Hierarquia")]
    public string nomeDoFilhoObjeto = "objeto"; 
    public string nomeDoPlane = "Plane";
    public string nomeSetaCima = "setacima";   
    public string nomeSetaBaixo = "setabaixo"; 

    [Header("Ajustes de Animação")]
    public float delaySairAoGanhar = 1.5f;

    public int index = -1; 

    private bool resolvido = false;
    private int[] simbolosIndices; 
    private bool aVer = false;
    private bool processandoResposta = false;
    public GameObject canvasGeral;

    public string TextoInteracao() => resolvido ? "" : "Fazer Puzzle";

    void Awake()
    {
        texto.SetActive(false);
        index = -1;
        simbolosIndices = new int[digitos.Length];
        
        for (int i = 0; i < digitos.Length; i++)
        {
            AplicarMaterialNoPlane(i);
        }
        AtualizarVisualDigitos();
    }

    void Update()
    {
        if (!aVer || processandoResposta) return;

        if (Input.GetKeyDown(teclaParaSair)) Sair();

        if (Input.GetKeyDown(teclaConfirmar)) StartCoroutine(VerificarSolucao());

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            MudarIndiceHorizontal(1);
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            MudarIndiceHorizontal(-1);

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
        
        string nomeSeta = (direcao > 0) ? nomeSetaCima : nomeSetaBaixo;
        Transform setaT = digitos[index].transform.Find(nomeSeta);
        
        if (setaT != null)
        {
            MeshRenderer rend = setaT.GetComponent<MeshRenderer>();
            if (rend != null) StartCoroutine(FlashCorSeta(rend, Color.green));
        }
    }

    // Corrigido: Piscar seta sem "bugar" a cor permanentemente
    IEnumerator FlashCorSeta(MeshRenderer rend, Color cor)
    {
        Color corOriginal = Color.black; // Ou a cor padrão das tuas setas
        rend.material.color = cor;
        yield return new WaitForSeconds(0.15f);
        rend.material.color = corOriginal;
    }

    IEnumerator VerificarSolucao()
    {
        processandoResposta = true;
        bool correto = true;

        for (int i = 0; i < digitos.Length; i++)
        {
            if (simbolosIndices[i] != solucaoCorreta[i]) correto = false;
        }

        // Piscar todos os planos (Verde se certo, Vermelho se errado)
        Color corFeedback = correto ? Color.green : Color.red;
        List<MeshRenderer> renderers = new List<MeshRenderer>();

        for (int i = 0; i < digitos.Length; i++)
        {
            Transform p = digitos[i].transform.Find(nomeDoFilhoObjeto + "/" + nomeDoPlane);
            if (p) renderers.Add(p.GetComponent<MeshRenderer>());
        }

        // Efeito de Piscar
        foreach (var r in renderers) r.material.SetColor("_EmissionColor", corFeedback * 2f);
        yield return new WaitForSeconds(0.5f);
        foreach (var r in renderers) r.material.SetColor("_EmissionColor", Color.black);

        if (correto)
        {
            resolvido = true;

            IAbrivel abrivel = objetoParaAbrir?.GetComponent<IAbrivel>();
            if (abrivel != null) abrivel.AbrirFechar();
            AudioManager.Instance.TocarSucesso();
            StartCoroutine(FinalizarPuzzle());
        }
        else
        {
            processandoResposta = false;
        }
    }

    void AplicarMaterialNoPlane(int i)
    {
        Transform targetPlane = digitos[i].transform.Find(nomeDoFilhoObjeto + "/" + nomeDoPlane);
        if (targetPlane != null)
        {
            MeshRenderer renderer = targetPlane.GetComponent<MeshRenderer>();
            if (renderer != null && materiaisSimbolos.Length > 0)
            {
                renderer.material = materiaisSimbolos[simbolosIndices[i]];
            }
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

    public void Interagir()
    {
        if (resolvido) return;
        aVer = true;
        texto.SetActive(true);
        index = 0;
        processandoResposta = false;
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
        texto.SetActive(false);
        index = -1;
        if (canvasGeral != null) canvasGeral.SetActive(true);
        if (camerapuzzle != null) camerapuzzle.gameObject.SetActive(false);
        cameraPrincipal.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        AtualizarVisualDigitos();
        StartCoroutine(LibertarMenu());
    }

    IEnumerator FinalizarPuzzle()
    {
        yield return new WaitForSeconds(delaySairAoGanhar);
        Sair();
        
        // Opcional: Desativa o script ou o componente de interação 
        // para o Raycast do jogador nem sequer o detetar mais.
        this.enabled = false; 
    }

    IEnumerator LibertarMenu()
    {
        yield return new WaitForEndOfFrame();
        GameManager.MenuOcupado = false;
        GameManager.InputBloqueado = false;
    }
}