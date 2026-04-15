using UnityEngine;
using System.Collections;

public class Bau : MonoBehaviour, IInteragivel, IAbrivel
{
    [Header("Referências")]
    public Transform tampa;        // a parte de cima do baú
    public Transform base_bau;     // a parte de baixo (só para o Gizmo)

    [Header("Configurações")]
    public float anguloAberto = -100f;  // negativo = abre para trás
    public float duracao = 0.5f;
    public float offsetDobradiça = 0.5f; // metade da profundidade da tampa

    [Header("Tranca")]
    public bool temTranca = false;
    private bool desbloqueado = false;

    private bool estaAberto = false;
    private bool aAnimar = false;

    public string TextoInteracao()
    {
        if (temTranca && !desbloqueado) return "Está trancado";
        return estaAberto ? "Fechar baú" : "Abrir baú";
    }

    public void Interagir()
    {
        if (temTranca && !desbloqueado) return;
        if (!aAnimar) StartCoroutine(AnimarTampa());
    }

    // Chamado pelo ItemRecetor quando usas a chave
    public void AbrirFechar()
    {
           Debug.Log("O método Abrir foi chamado!");
        desbloqueado = true;
        if (!aAnimar) StartCoroutine(AnimarTampa());
    }

    private Vector3 PontoDobradiça()
    {
        // Dobradiça na parte de trás da tampa
        return tampa.position + tampa.forward * offsetDobradiça;
    }

    IEnumerator AnimarTampa()
    {
        aAnimar = true;

        float angulo = estaAberto ? -anguloAberto : anguloAberto;
        float girado = 0f;

        while (Mathf.Abs(girado) < Mathf.Abs(angulo))
        {
            float passo = (angulo / duracao) * Time.deltaTime;

            if (Mathf.Abs(girado + passo) > Mathf.Abs(angulo))
                passo = angulo - girado;

            // Roda a tampa à volta da dobradiça de trás
            tampa.RotateAround(PontoDobradiça(), tampa.right, passo);
            girado += passo;
            yield return null;
        }

        estaAberto = !estaAberto;
        aAnimar = false;
    }

    void OnDrawGizmos()
    {
        if (tampa == null) return;

        // Ponto da dobradiça — amarelo como na porta
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(PontoDobradiça(), 0.05f);

        // Linha da dobradiça
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(
            PontoDobradiça() - tampa.right * 0.3f,
            PontoDobradiça() + tampa.right * 0.3f
        );

        // Outline da tampa — verde se aberto, vermelho se fechado
        if (base_bau != null)
        {
            Gizmos.color = estaAberto ? Color.green : Color.red;
            Gizmos.DrawWireCube(base_bau.position, base_bau.localScale);
            Gizmos.DrawWireCube(tampa.position, tampa.localScale);
        }
    }
}