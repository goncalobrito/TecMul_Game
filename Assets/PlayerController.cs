using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimento")]
    public CharacterController controller;
    public float speed = 12f;
    public float sprintSpeed = 18f;

    [Header("Salto e Gravidade")]
    public float jumpHeight = 1.5f;
    public float gravity = -20f;

    [Header("Crouch")]
    public float alturaNormal = 2f;
    public float alturaCrouch = 1f;
    public float speedCrouch = 6f;
    public Transform cameraTransform;

    private Vector3 velocity;
    private bool estaNoChao;
    private bool estaCrouch = false;

    void Update()
    {
        // --- CHÃO com Raycast (deteta qualquer objeto sólido) ---
        estaNoChao = Physics.Raycast(transform.position, Vector3.down, 
                        controller.height / 2f + 0.1f);

        if (estaNoChao && velocity.y < 0)
            velocity.y = -2f;

        // --- MOVIMENTO ---
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        bool aSprinter = Input.GetKey(KeyCode.LeftShift) && !estaCrouch;
        float velocidadeAtual = estaCrouch ? speedCrouch : (aSprinter ? sprintSpeed : speed);

        controller.Move(move * velocidadeAtual * Time.deltaTime);

        // --- SALTO ---
        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao && !estaCrouch)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // --- CROUCH ---
        if (Input.GetKeyDown(KeyCode.LeftControl))
            ToggleCrouch();

        // --- GRAVIDADE ---
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void ToggleCrouch()
    {
        estaCrouch = !estaCrouch;

        if (estaCrouch)
        {
            controller.height = alturaCrouch;
            cameraTransform.localPosition = new Vector3(0, alturaCrouch * 0.4f, 0);
        }
        else
        {
            // verifica se há teto antes de se levantar
            if (!Physics.Raycast(transform.position, Vector3.up, alturaNormal))
            {
                controller.height = alturaNormal;
                cameraTransform.localPosition = new Vector3(0, alturaNormal * 0.4f, 0);
            }
            else
            {
                estaCrouch = true;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Mostra o raycast no editor
        Gizmos.color = estaNoChao ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, 
                        transform.position + Vector3.down * (controller.height / 2f + 0.1f));
    }
}