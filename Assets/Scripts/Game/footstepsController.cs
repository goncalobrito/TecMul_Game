using UnityEngine;

public class FootstepController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private AudioSource source;
    private float timer = 0f;

    [Header("Volumes")]
    public float volumeNormal = 0.8f;
    public float volumeCrouch = 0.3f;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        source = gameObject.AddComponent<AudioSource>();
        source.spatialBlend = 1f; // som 3D
        source.volume = volumeNormal;
    }

    void Update()
    {
        if (GameManager.InputBloqueado) return;

        bool estaAMover = playerMovement.EstaAMover();
        if (!estaAMover)
        {
            timer = 0f;
            return;
        }

        float intervalo = AudioManager.Instance.intervaloPassos;
        if (playerMovement.EstaCrouch)
            intervalo = AudioManager.Instance.intervaloPassosCrouch;
        else if (playerMovement.EstaSprint)
            intervalo = AudioManager.Instance.intervaloPassosSprint;

        timer += Time.deltaTime;
        if (timer >= intervalo)
        {
            timer = 0f;
            float vol = playerMovement.EstaCrouch ? volumeCrouch : volumeNormal;
            source.PlayOneShot(AudioManager.Instance.somPasso, vol);
        }
    }
}