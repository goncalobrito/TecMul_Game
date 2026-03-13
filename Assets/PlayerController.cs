using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidade = 5f;
    public float forcaPulo = 2f;
    public float gravidade = -9.81f;

    [Header("Configurações do Rato")]
    public float sensibilidadeRato = 200f;
    public Transform cameraTransform;

    private CharacterController controller;
    private Vector3 velocidadeVertical;
    private float rotacaoX = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        // Esconde o rato e prende-o no centro do ecrã
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        MoverJogador();
        RodarCamera();
    }

    void MoverJogador()
    {
        // Verifica se o jogador está no chão
        bool noChao = controller.isGrounded;
        if (noChao && velocidadeVertical.y < 0)
        {
            velocidadeVertical.y = -2f;
        }

        // Input de movimento (Teclado)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movimento = transform.right * x + transform.forward * z;
        controller.Move(movimento * velocidade * Time.deltaTime);

        // Lógica de Pulo
        if (Input.GetButtonDown("Jump") && noChao)
        {
            velocidadeVertical.y = Mathf.Sqrt(forcaPulo * -2f * gravidade);
        }

        // Aplicar Gravidade
        velocidadeVertical.y += gravidade * Time.deltaTime;
        controller.Move(velocidadeVertical * Time.deltaTime);
    }

    void RodarCamera()
    {
        // Input do Rato
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadeRato * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadeRato * Time.deltaTime;

        // Roda o corpo do jogador (Esquerda/Direita)
        transform.Rotate(Vector3.up * mouseX);

        // Roda a câmara (Cima/Baixo) com limite de 90 graus
        rotacaoX -= mouseY;
        rotacaoX = Mathf.Clamp(rotacaoX, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(rotacaoX, 0f, 0f);
    }
}