using UnityEngine;

public class boxTRIGGER : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se quem colidiu tem a tag "Player"
        if (other.CompareTag("Player"))
        {
            ColorManager.Instance.corAtual = new Color32(28, 54, 255, 255);

            ColorManager.Instance.intensidade = 10f;
        }
    }
}
