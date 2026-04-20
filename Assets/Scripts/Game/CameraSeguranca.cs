using UnityEngine;
using System.Collections;
using System.Collections.Generic; // Necessário para usar List

public class CameraSeguranca : MonoBehaviour, IInteragivel
{
    [Header("Configurações de Câmaras")]
    public List<Camera> camerasSeguranca; 
    public Camera cameraPrincipal;
    private Camera cameraAtual; // A câmara que está a ser mostrada agora
    private int indiceAtual = 0;

    [Header("Configurações de UI")]
    public GameObject canvasGeral;           // UI do Jogo (Hotbar, Mira)
    public GameObject canvasCameraSeguranca; // UI da Câmara (Botões de troca)
    
    public KeyCode teclaParaSair = KeyCode.Escape;
    private bool aVer = false;

    public string TextoInteracao() => "Ver câmara";

    void Awake()
    {
        // Garante que todas as câmaras de segurança começam desligadas
        foreach (Camera cam in camerasSeguranca)
        {
            if (cam != null) cam.gameObject.SetActive(false);
        }
        
        if (canvasCameraSeguranca != null) canvasCameraSeguranca.SetActive(false);
    }

    void Update()
    {
        if (aVer && Input.GetKeyDown(teclaParaSair))
            Sair();
    }

    public void Interagir()
    {
        if (camerasSeguranca.Count == 0) return;

        aVer = true;
        GameManager.MenuOcupado = true;
        GameManager.InputBloqueado = true;

        // 1. Alternar UI
        if (canvasGeral != null) canvasGeral.SetActive(false);
        if (canvasCameraSeguranca != null) canvasCameraSeguranca.SetActive(true);

        // 2. Alternar Câmaras
        cameraPrincipal.gameObject.SetActive(false);
        indiceAtual = 0;
        cameraAtual = camerasSeguranca[indiceAtual];
        cameraAtual.gameObject.SetActive(true);

        // 3. Ativar o rato
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Sair()
    {
        aVer = false;
        
        if (cameraAtual != null) cameraAtual.gameObject.SetActive(false);
        cameraPrincipal.gameObject.SetActive(true);

        if (canvasGeral != null) canvasGeral.SetActive(true);
        if (canvasCameraSeguranca != null) canvasCameraSeguranca.SetActive(false);

        // 4. Esconder o rato
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine(LibertarMenu());
    }

    // Método para o botão "Próxima"
    public void ProximaCamera()
    {
        cameraAtual.gameObject.SetActive(false);
        
        indiceAtual = (indiceAtual + 1) % camerasSeguranca.Count;
        
        cameraAtual = camerasSeguranca[indiceAtual];
        cameraAtual.gameObject.SetActive(true);
    }

    // Método para o botão "Anterior"
    public void CameraAnterior()
    {
        cameraAtual.gameObject.SetActive(false);
        
        indiceAtual = (indiceAtual - 1 + camerasSeguranca.Count) % camerasSeguranca.Count;
        
        cameraAtual = camerasSeguranca[indiceAtual];
        cameraAtual.gameObject.SetActive(true);
    }

    IEnumerator LibertarMenu()
    {
        yield return new WaitForEndOfFrame();
        GameManager.MenuOcupado = false;
        GameManager.InputBloqueado = false;
    }
}