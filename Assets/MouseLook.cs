using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Alvos")]
    public Transform headBone;
    public Transform playerBody;

    [Header("Configurações de Visão")]
    public float mouseSensitivity = 0.1f;
    public Vector3 eyeOffset = new Vector3(0, 0.1f, 0.1f);

    [Header("Suavização")]
    public float suavizacao = 20f;

    float xRotation = 0f;
    float yRotation = 0f;
    private PlayerMovement playerMovement;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        yRotation = playerBody.eulerAngles.y;
        playerMovement = playerBody.GetComponent<PlayerMovement>();
    }

    void LateUpdate()
    {
        if (headBone == null || playerBody == null) return;
        if (playerMovement == null) return;
        if (GameManager.InputBloqueado) return;

        // 1. POSIÇÃO suavizada — X e Z seguem o corpo, Y segue o osso
        Vector3 posAlvo = new Vector3(
            playerBody.position.x,
            headBone.position.y,
            playerBody.position.z
        ) + playerBody.TransformDirection(eyeOffset);

        transform.position = Vector3.Lerp(
            transform.position,
            posAlvo,
            suavizacao * Time.deltaTime
        );

        // 2. ROTAÇÃO
        xRotation -= playerMovement.lookInput.y * mouseSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        yRotation += playerMovement.lookInput.x * mouseSensitivity;

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // 3. CORPO só roda para os lados
        playerBody.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}