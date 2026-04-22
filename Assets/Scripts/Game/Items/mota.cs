using UnityEngine;
using System.Collections;

public class Mota : MonoBehaviour, IInteragivel
{
    [Header("Efeitos Visuais")]
    [SerializeField] private ParticleSystem particulasEscape;

    [SerializeField] private float velocidadeInicial = 20f;
    [SerializeField] private float velocidadeFinal = 2f;
    [SerializeField] private float duracaoDoEfeito = 1.5f; // Tempo até as partículas "cansarem"

    public string TextoInteracao() => "BRAAP BRAPP BRAPP";

    public void Interagir() => Acelarar();

    void Acelarar()
    {
        AudioManager.Instance.Tocarmota();

        if (particulasEscape != null)
        {
            StopAllCoroutines(); 
            StartCoroutine(EfeitoAcelerador());
        }

        
    }

    private IEnumerator EfeitoAcelerador()
    {
        var mainModule = particulasEscape.main;
        float tempoPassado = 0;

        // Ativa as partículas
        particulasEscape.Play();

        while (tempoPassado < duracaoDoEfeito)
        {
            tempoPassado += Time.deltaTime;
            
            // Calcula o progresso de 0 a 1
            float progresso = tempoPassado / duracaoDoEfeito;
            
            // Invertemos o Lerp: começa na velocidade alta e vai para a baixa
            // Usei Lerp com uma curva suave (SmoothStep) para parecer mais natural
            float t = progresso * progresso * (3f - 2f * progresso); 
            mainModule.startSpeed = Mathf.Lerp(velocidadeInicial, velocidadeFinal, t);

            yield return null; 
        }

        mainModule.startSpeed = velocidadeFinal;
    }
}