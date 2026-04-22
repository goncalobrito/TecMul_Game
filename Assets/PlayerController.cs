using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private AnimationHandler animationHandler;

    [Header("Componentes")]
    public CharacterController controller;
    private PlayerInput playerInput;
    private InputAction sprintAction;

    [Header("Movimento")]
    public float speed = 3f;
    public float sprintSpeed = 5f;
    public float speedCrouch = 1.5f;
    private Vector2 inputMovimento;

    [Header("Salto e Gravidade")]
    public float jumpHeight = 1.5f;
    public float gravity = -20f;
    private Vector3 velocity;
    private bool estaNoChao;
    private bool estaNoAr = false;

    [Header("Crouch")]
    public float alturaNormal = 2f;
    public float alturaCrouch = 1f;
    private bool estaCrouch = false;

    public Vector2 lookInput { get; private set; }
    public bool EstaCrouch => estaCrouch;
    public bool EstaSprint => sprintAction != null && sprintAction.IsPressed();
    public bool EstaAMover() => inputMovimento.magnitude > 0.1f;

    void Awake()
    {
        animationHandler = GetComponent<AnimationHandler>();
        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
            sprintAction = playerInput.actions["Sprint"];
    }

    void Update()
    {
        // Se o input bloquear, forçamos o movimento a parar para não andar sozinho
        if (GameManager.InputBloqueado)
        {
            inputMovimento = Vector2.zero;
            // Opcional: se quiseres que a gravidade continue a atuar mesmo no puzzle:
            VerificarChao();
            ProcessarGravidade();
            AtualizarAnimator(false);
            return;
        }

        bool aSprinter = EstaSprint;

        VerificarChao();
        ProcessarMovimento(aSprinter);
        ProcessarGravidade();
        AtualizarAnimator(aSprinter);
    }

    public void OnMove(InputValue value)
    {
        if (GameManager.InputBloqueado) return;
        inputMovimento = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (GameManager.InputBloqueado) return; // IMPEDE O SALTO NO PUZZLE
        if (value.isPressed) TentarSaltar();
    }

    public void OnCrouch(InputValue value)
    {
        if (GameManager.InputBloqueado) return;
        if (value.isPressed) ToggleCrouch();
    }

    public void OnLook(InputValue value)
    {
        // Já tinhas esta bem feita!
        if (GameManager.InputBloqueado) { lookInput = Vector2.zero; return; }
        lookInput = value.Get<Vector2>();
    }

    void ProcessarMovimento(bool aSprinter)
    {
        Vector3 direcao = transform.right * inputMovimento.x + transform.forward * inputMovimento.y;
        if (direcao.magnitude > 1f) direcao.Normalize();

        float velocidade;
        if (estaCrouch) velocidade = speedCrouch;
        else if (aSprinter && inputMovimento.y > 0) velocidade = sprintSpeed; // Só corre para a frente
        else velocidade = speed;

        controller.Move(direcao * velocidade * Time.deltaTime);
    }

    void AtualizarAnimator(bool aSprinter)
    {
        if (animationHandler == null) return;

        bool isRunning = aSprinter && inputMovimento.y > 0.1f && !estaCrouch;

        Vector2 inputNorm = inputMovimento.magnitude > 0.1f ? inputMovimento.normalized : Vector2.zero;
        if (estaCrouch) inputNorm *= 0.5f;

        // Única chamada necessária para animação
        animationHandler.UpdateAnimation(inputNorm, isRunning, estaCrouch, estaNoAr);
    }

    void TentarSaltar()
    {
        if (estaNoChao && !estaCrouch)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            estaNoAr = true;
        }
    }

    void VerificarChao()
    {
        Vector3 bottom = transform.position + controller.center + Vector3.down * (controller.height / 2f);
        estaNoChao = Physics.CheckSphere(bottom, 0.15f);

        if (estaNoChao && velocity.y < 0)
        {
            velocity.y = -2f;
            estaNoAr = false;
        }
    }

    void ToggleCrouch()
    {
        if (!estaCrouch)
        {
            estaCrouch = true;
            controller.height = alturaCrouch;
            controller.center = new Vector3(controller.center.x, -0.12f, controller.center.z);
        }
        else
        {
            LevantarCrouch();
        }
    }

    void LevantarCrouch()
    {
        float raioVerificacao = alturaNormal - 0.1f;
        bool temTeto = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.up, raioVerificacao);

        if (!temTeto)
        {
            estaCrouch = false;
            controller.height = alturaNormal;
            controller.center = new Vector3(controller.center.x, -0.05f, controller.center.z);
        }
    }

    void ProcessarGravidade()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        if (controller == null) return;
        Vector3 bottom = transform.position + controller.center + Vector3.down * (controller.height / 2f);
        Gizmos.color = estaNoChao ? Color.green : Color.red;
        Gizmos.DrawWireSphere(bottom, 0.15f);
    }
}