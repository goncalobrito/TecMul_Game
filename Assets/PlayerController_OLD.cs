/*
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    [Header("Movimento")]
    public CharacterController controller;
    public float speed = 3f;
    public float sprintSpeed = 5f;

    [Header("Salto e Gravidade")]
    public float jumpHeight = 1.5f;
    public float gravity = -20f;

    [Header("Crouch")]
    public float alturaNormal = 2f;
    public float alturaCrouch = 1f;
    public float speedCrouch = 1.5f;
    public Transform cameraTransform;
    private float cameraAlturaНormal;
    private float cameraAlturaCrouch;

    [Header("Animações")]
    private Animator animator;

    private Vector3 velocity;
    private bool estaNoChao;
    private bool estaCrouch = false;
    private bool estaNoAr = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Guarda a altura inicial da câmera para usar como referência
        cameraAlturaНormal = cameraTransform.localPosition.y;
        cameraAlturaCrouch = cameraAlturaНormal * (alturaCrouch / alturaNormal);
    }

    void Update()
    {
        // Deteção do chão baseada no centro do controller
        Vector3 bottom = transform.position + controller.center + Vector3.down * (controller.height / 2f);
        estaNoChao = Physics.CheckSphere(bottom, 0.15f);

        if (estaNoChao && velocity.y < 0)
            velocity.y = -2f;

        // --- MOVIMENTO ---
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        bool aSprinter = Input.GetKey(KeyCode.LeftShift);

        // Sprint enquanto crouch → levanta automaticamente
        if (aSprinter && estaCrouch)
            LevantarCrouch();

        float velocidadeAtual = estaCrouch ? speedCrouch : (aSprinter ? sprintSpeed : speed);
        controller.Move(move * velocidadeAtual * Time.deltaTime);

        // --- SALTO ---
        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao && !estaCrouch)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("IsJumping", true);
            estaNoAr = true;
        }

        // Detetar aterragem
        if (estaNoAr && estaNoChao && velocity.y < 0)
        {
            animator.SetBool("IsJumping", false);
            estaNoAr = false;
        }

        // --- CROUCH ---
        if (Input.GetKeyDown(KeyCode.LeftControl))
            ToggleCrouch();

        // --- GRAVIDADE ---
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // --- ANIMATOR ---
        float inputMagnitude = new Vector3(x, 0, z).magnitude;
        float speedParam = inputMagnitude * velocidadeAtual;
        animator.SetFloat("Speed", speedParam);
        animator.SetBool("IsCrouching", estaCrouch);
    }

    void ToggleCrouch()
    {
        if (!estaCrouch) // Vai agachar
        {
            estaCrouch = true;
            controller.height = alturaCrouch;
            // Ajusta o centro para que a base (pés) continue no mesmo lugar
            //controller.center = new Vector3(0, alturaCrouch / 2f, 0);
            cameraTransform.localPosition = new Vector3(0, cameraAlturaCrouch, 0);
        }
        else // Tentar levantar
        {
            LevantarCrouch();
        }
    }

    void LevantarCrouch()
    {
        // Raio começa um pouco acima da base para não bater no próprio chão
        // O comprimento do raio deve ser a altura normal
        float raioVerificacao = alturaNormal - 0.1f;

        // Verifica se há algo acima da cabeça
        bool temTeto = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.up, raioVerificacao);

        if (!temTeto)
        {
            estaCrouch = false;
            controller.height = alturaNormal;
            // Ajusta o centro para a altura normal
            //controller.center = new Vector3(0, alturaNormal / 2f, 0);
            cameraTransform.localPosition = new Vector3(0, cameraAlturaНormal, 0);
        }
        else
        {
            Debug.Log("Não posso levantar, tem algo em cima!");
        }
    }
    void OnDrawGizmosSelected()
    {
        if (controller == null) return;
        Vector3 bottom = transform.position + controller.center + Vector3.down * (controller.height / 2f);
        Gizmos.color = estaNoChao ? Color.green : Color.red;
        Gizmos.DrawWireSphere(bottom, 0.15f);
    }
    
}
*/