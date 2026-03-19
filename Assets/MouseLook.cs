using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    float xRotation = 0f;

    void Start()
    {
        // Prende o rato no centro do ecrã e esconde-o
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotação Vertical (Câmara sobe e desce)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Impede que a câmara dê a volta completa

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotação Horizontal (O corpo do jogador roda para os lados)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}