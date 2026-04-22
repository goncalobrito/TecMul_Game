using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Configurações de Sensibilidade")]
    public Slider sliderSens;

    void Start()
    {
        // Inicializa o slider com o valor salvo (ou 0.1 que é o padrão do seu script)
        if (sliderSens != null)
        {
            sliderSens.value = PlayerPrefs.GetFloat("SensibilidadeSalva", 0.1f);
            sliderSens.onValueChanged.AddListener(SalvarSensibilidade);
        }
    }

    public void SalvarSensibilidade(float valor)
    {
        PlayerPrefs.SetFloat("SensibilidadeSalva", valor);
        
        // Se estivermos na cena do jogo, aplica na hora
        AplicarSensibilidadeNaCamera(valor);
    }

    // Esta função procura a câmera e altera o "mouseSensitivity" dela
    private void AplicarSensibilidadeNaCamera(float valor)
    {
        CameraController cam = Object.FindAnyObjectByType<CameraController>();
        if (cam != null)
        {
            cam.mouseSensitivity = valor;
        }
    }

    public void JogarAgora()
    {
        // Quando carregar a cena, precisamos garantir que a câmera leia o valor salvo
        SceneManager.sceneLoaded += AoCarregarCena; 
        SceneManager.LoadScene("Game");
    }

    // Evento chamado automaticamente quando a cena "Game" termina de carregar
    private void AoCarregarCena(Scene cena, LoadSceneMode modo)
    {
        float sensSalva = PlayerPrefs.GetFloat("SensibilidadeSalva", 0.1f);
        AplicarSensibilidadeNaCamera(sensSalva);
        
        // Remove o evento para não acumular
        SceneManager.sceneLoaded -= AoCarregarCena;
    }

    public void Sair()
    {
        Application.Quit();
    }
}