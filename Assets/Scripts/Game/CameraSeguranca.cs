using UnityEngine;
using System.Collections;

public class CameraSeguranca : MonoBehaviour, IInteragivel
{
    public Camera cameraSeguranca;
    public Camera cameraPrincipal;
    public KeyCode teclaParaSair = KeyCode.Escape;

    private bool aVer = false;

    public string TextoInteracao() => "Ver câmara";

    void Awake()
    {
        cameraSeguranca.gameObject.SetActive(false);
    }

    void Update()
    {
        if (aVer && Input.GetKeyDown(teclaParaSair))
            Sair();
    }

    public void Interagir()
    {
        aVer = true;
        GameManager.MenuOcupado = true;
        GameManager.InputBloqueado = true;
        cameraPrincipal.gameObject.SetActive(false);
        cameraSeguranca.gameObject.SetActive(true);
    }

    void Sair()
    {
        aVer = false;
        cameraSeguranca.gameObject.SetActive(false);
        cameraPrincipal.gameObject.SetActive(true);
        StartCoroutine(LibertarMenu());
    }

    IEnumerator LibertarMenu()
    {
        yield return new WaitForEndOfFrame();
        GameManager.MenuOcupado = false;
        GameManager.InputBloqueado = false;
    }
}