using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Passos")]

    public AudioClip somPasso;

    public float intervaloPassos = 0.4f;

    public float intervaloPassosCrouch = 0.6f;

    public float intervaloPassosSprint = 0.25f;
    // ... (restante dos teus Headers)
    [Header("Interações")]
    public AudioClip somPorta;
    public AudioClip somChave;
    public AudioClip somBotao;
    public AudioClip somErro;
    public AudioClip somSucesso;
    public AudioClip somApanharItem;
    public AudioClip somLanterna;
    public AudioClip somElevador;

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

    // --- FUNÇÕES DE CONTROLO ---

    public void PararSom()
    {
        source.loop = false; // Desativar o loop antes de parar
        source.Stop();
    }

    public void Tocar(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
            source.PlayOneShot(clip, volume);
    }

    // --- MÉTODOS ESPECÍFICOS ---

    public void TocarPorta() => Tocar(somPorta);
    public void TocarChave() => Tocar(somChave);
    public void TocarBotao() => Tocar(somBotao);
    public void TocarErro() => Tocar(somErro, 0.8f);
    public void TocarSucesso() => Tocar(somSucesso);
    public void TocarApanharItem() => Tocar(somApanharItem, 0.7f);
    public void TocarLanterna() => Tocar(somLanterna, 0.7f);

    // ALTERADO: Este método agora configura o Source para Loop
    public void TocarElevador()
    {
        if (somElevador != null)
        {
            source.clip = somElevador;
            source.volume = 0.3f;
            source.loop = true; // Fundamental para o motor não parar
            source.Play();
        }
    }
}