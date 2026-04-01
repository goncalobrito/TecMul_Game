using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    [Header("Anti-puxada")]
    public float maxDeltaPerFrame = 10f; // corta picos acima deste valor

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // GetAxisRaw não tem smoothing acumulado — mais preciso
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Corta picos — se o delta for absurdo, ignora
        mouseX = Mathf.Clamp(mouseX, -maxDeltaPerFrame, maxDeltaPerFrame);
        mouseY = Mathf.Clamp(mouseY, -maxDeltaPerFrame, maxDeltaPerFrame);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    // Garante que o lock é reativado se a janela perder foco
    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
            Cursor.lockState = CursorLockMode.Locked;
    }
}