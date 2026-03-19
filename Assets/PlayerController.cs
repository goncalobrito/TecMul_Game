using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
    }
}

<<<<<<< Updated upstream
//testecomit
=======
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


//testeeeeeeeeeeeeeeeeee
>>>>>>> Stashed changes
