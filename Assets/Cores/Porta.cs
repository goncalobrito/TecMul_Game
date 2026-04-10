using UnityEngine;
using System.Collections;

public class PortaMecanismo : MonoBehaviour, IInteragivel
{
    public float anguloAberta = 90f;
    public float duracao = 0.6f;
    public float offsetDobradiça = 0.5f;
    public bool temTranca = false; // se true, só abre via ItemRecetor

    private bool estaAberta = false;
    private bool aAnimar = false;

    public string TextoInteracao() => estaAberta ? "Fechar porta" : "Abrir porta";

    private Vector3 PontoDobradiça() =>
        transform.position + transform.right * -offsetDobradiça;

    public void Interagir()
    {
        if (!temTranca)
            AbrirFechar();
    }

    public void AbrirFechar()
    {
        if (!aAnimar)
        {
            AudioManager.Instance.TocarPorta();
            StartCoroutine(AnimarPorta());
        }
    }

    IEnumerator AnimarPorta()
    {
        aAnimar = true;
        float angulo = estaAberta ? -anguloAberta : anguloAberta;
        float girado = 0f;

        while (Mathf.Abs(girado) < Mathf.Abs(angulo))
        {
            float passo = (angulo / duracao) * Time.deltaTime;

            if (Mathf.Abs(girado + passo) > Mathf.Abs(angulo))
                passo = angulo - girado;

            transform.RotateAround(PontoDobradiça(), Vector3.up, passo);
            girado += passo;
            yield return null;
        }

        estaAberta = !estaAberta;
        aAnimar = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(PontoDobradiça(), 0.05f);
    }
}