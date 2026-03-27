using UnityEngine;

public class PortaMecanismo : MonoBehaviour
{
    private bool aberta = false;
    private Quaternion rotOriginal;
    private Quaternion rotDestino;
    public float velocidade = 5f;
    public float anguloAbertura = 90f;

    void Start()
    {
        rotOriginal = transform.rotation;
        rotDestino = Quaternion.AngleAxis(anguloAbertura, Vector3.up) * rotOriginal;
    }

    public void AbrirFechar()
    {
        aberta = !aberta;
    }

    void Update()
    {
        Quaternion alvo = aberta ? rotDestino : rotOriginal;
        transform.rotation = Quaternion.Slerp(transform.rotation, alvo, Time.deltaTime * velocidade);
    }
}