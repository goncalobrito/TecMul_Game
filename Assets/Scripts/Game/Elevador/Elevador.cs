using UnityEngine;
using System.Collections;

public class ElevadorController : MonoBehaviour, IAbrivel
{
    [Header("Portas")]
    public Transform portaEsquerda;
    public Transform portaDireita;
    public Vector3 deslocamentoPorta = new Vector3(1.5f, 0, 0);
    public float velocidadeAbertura = 2f;

    [Header("Sensação de Movimento")]
    public float tempoDeDescida = 4.0f;
    public float intensidadeVibracao = 0.05f;

    [Header("Iluminação")]
    public Light luzTeto;
    public float intensidadeNormal = 1f;
    public float intensidadeChegada = 1.3f;
    public bool piscarNaDescida = true;

    private bool jaAbriu = false;
    private bool jaFechou = false; // Nova flag para garantir que só fecha uma vez
    private Vector3 posInicialEsq, posFinalEsq;
    private Vector3 posInicialDir, posFinalDir;
    private Transform cameraPlayer;

    void Start()
    {
        cameraPlayer = Camera.main.transform;

        if (portaEsquerda && portaDireita)
        {
            posInicialEsq = portaEsquerda.localPosition;
            posFinalEsq = posInicialEsq - deslocamentoPorta;
            posInicialDir = portaDireita.localPosition;
            posFinalDir = posInicialDir + deslocamentoPorta;
        }

        if (luzTeto != null) luzTeto.intensity = intensidadeNormal;
    }

    public void AbrirFechar()
    {
        if (!jaAbriu) StartCoroutine(SequenciaElevador());
    }


    IEnumerator SequenciaElevador()
    {
        jaAbriu = true;
        float timer = 0;
        Vector3 posicaoOriginalCam = cameraPlayer.localPosition;

        AudioManager.Instance.TocarElevador(); 

        while (timer < tempoDeDescida)
        {
            timer += Time.deltaTime;
            float x = Random.Range(-1f, 1f) * intensidadeVibracao;
            float y = Random.Range(-1f, 1f) * intensidadeVibracao;
            cameraPlayer.localPosition = new Vector3(posicaoOriginalCam.x + x, posicaoOriginalCam.y + y, posicaoOriginalCam.z);

            if (piscarNaDescida && luzTeto != null)
            {
                float chance = Random.value; 
                if (chance > 0.8f) luzTeto.intensity = 0.2f; 
                else if (chance > 0.5f) luzTeto.intensity = intensidadeNormal; 
                else luzTeto.intensity = 0.002f; 
            }
            yield return null;
        }

        cameraPlayer.localPosition = posicaoOriginalCam;

        if (AudioManager.Instance != null) 
        {
             AudioManager.Instance.PararSom();
             AudioManager.Instance.TocarSucesso();
        }

        if (luzTeto != null) luzTeto.intensity = intensidadeChegada;

        float progresso = 0;
        while (progresso < 1f)
        {
            progresso += Time.deltaTime * velocidadeAbertura;
            portaEsquerda.localPosition = Vector3.Lerp(posInicialEsq, posFinalEsq, progresso);
            portaDireita.localPosition = Vector3.Lerp(posInicialDir, posFinalDir, progresso);
            yield return null;
        }
    }

    public void AtivarFechoPelaSaida()
    {
        if (jaAbriu && !jaFechou)
        {
            StopAllCoroutines(); 
            StartCoroutine(FecharPortas());
        }
    }

    IEnumerator FecharPortas()
    {
        jaFechou = true;
        float progresso = 0;
        
        // Som de fecho ou de sucesso novamente
        if (AudioManager.Instance != null) AudioManager.Instance.TocarSucesso();

        // Pegamos a posição atual das portas (caso o player saia enquanto elas ainda abrem)
        Vector3 posAtualEsq = portaEsquerda.localPosition;
        Vector3 posAtualDir = portaDireita.localPosition;

        while (progresso < 1f)
        {
            progresso += Time.deltaTime * velocidadeAbertura;
            portaEsquerda.localPosition = Vector3.Lerp(posAtualEsq, posInicialEsq, progresso);
            portaDireita.localPosition = Vector3.Lerp(posAtualDir, posInicialDir, progresso);
            yield return null;
        }
        
        // Opcional: Desligar a luz ou mudar para vermelho para indicar que está trancado
        if (luzTeto != null) luzTeto.intensity = 0.1f;
        Debug.Log("Elevador trancado.");
    }
}