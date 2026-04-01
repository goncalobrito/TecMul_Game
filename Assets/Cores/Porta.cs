using UnityEngine;
using System.Collections;

public class PortaMecanismo : MonoBehaviour
{
    public float anguloAberta = 90f;
    public float duracao = 0.6f;

    // Offset da dobradiça relativo ao centro da porta
    // Se a porta tem largura 1, a dobradiça está a 0.5 para a esquerda
    public float offsetDobradiça = 0.5f;

    private bool estaAberta = false;
    private bool aAnimar = false;

    // Calcula o ponto da dobradiça automaticamente em runtime
    private Vector3 PontoDobradiça()
    {
        return transform.position + transform.right * -offsetDobradiça;
    }

    public void AbrirFechar()
    {
        if (!aAnimar)
            StartCoroutine(AnimarPorta());
    }

    IEnumerator AnimarPorta()
    {
        aAnimar = true;

        float angulo = estaAberta ? -anguloAberta : anguloAberta;
        float girado = 0f;

        while (Mathf.Abs(girado) < Mathf.Abs(angulo))
        {
            float passo = (angulo / duracao) * Time.deltaTime;

            // Não ultrapassar o ângulo final
            if (Mathf.Abs(girado + passo) > Mathf.Abs(angulo))
                passo = angulo - girado;

            // Roda à volta do ponto da dobradiça calculado
            transform.RotateAround(PontoDobradiça(), Vector3.up, passo);
            girado += passo;
            yield return null;
        }

        estaAberta = !estaAberta;
        aAnimar = false;
    }

    // Desenha o pivot no editor para poderes ver onde está
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(PontoDobradiça(), 0.05f);
    }
}