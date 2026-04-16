using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [Header("Configurações de Velocidade")]
    public float sensitivity = 100f;

    [Header("Limites de Rotação (Graus)")]
    public float minAngle = -45f;
    public float maxAngle = 45f;

    private float currentYRotation = 0f;

    void Start()
    {
        // Inicializa com a rotação atual da câmara para evitar "pulos"
        currentYRotation = transform.localEulerAngles.y;
        
        // Ajuste para lidar com ângulos da Unity (0-360)
        if (currentYRotation > 180) currentYRotation -= 360f;
    }

    void Update()
    {
        // Obtém o input das setas (ou A/D)
        // O "Horizontal" retorna -1 (esquerda), 1 (direita) ou 0
        float input = Input.GetAxis("Horizontal");

        if (input != 0)
        {
            // Calcula a nova rotação baseada no tempo (Time.deltaTime)
            currentYRotation += input * sensitivity * Time.deltaTime;

            // Aplica o limite (Clamp)
            currentYRotation = Mathf.Clamp(currentYRotation, minAngle, maxAngle);

            // Aplica a rotação ao transform
            transform.localRotation = Quaternion.Euler(25, currentYRotation, 0);
        }
    }
}