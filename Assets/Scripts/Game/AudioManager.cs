using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Passos")]
    public AudioClip somPasso;
    public float intervaloPassos = 0.4f;
    public float intervaloPassosCrouch = 0.6f;
    public float intervaloPassosSprint = 0.25f;

    [Header("Interações")]
    public AudioClip somPorta;
    public AudioClip somChave;
    public AudioClip somBotao;
    public AudioClip somErro;
    public AudioClip somSucesso;
    public AudioClip somApanharItem;

    private AudioSource source;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        source = GetComponent<AudioSource>();
    }

    public void Tocar(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
            source.PlayOneShot(clip, volume);
    }

    public void TocarPorta() => Tocar(somPorta);
    public void TocarChave() => Tocar(somChave);
    public void TocarBotao() => Tocar(somBotao);
    public void TocarErro() => Tocar(somErro, 0.8f);
    public void TocarSucesso() => Tocar(somSucesso);
    public void TocarApanharItem() => Tocar(somApanharItem, 0.7f);
}