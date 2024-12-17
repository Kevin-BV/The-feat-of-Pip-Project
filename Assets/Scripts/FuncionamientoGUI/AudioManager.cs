using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Range(0f, 1f)]
    public float sfxVolume = 1f;
    [Range(0f, 1f)]
    public float musicVolume = 1f;

    void Awake()
    {
        // Asegurar que solo exista una instancia del AudioManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
        }
        else
        {
            Destroy(gameObject);
        }

        // Cargar configuraciones previas de volumen
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);  // Valor predeterminado de 1 si no se encuentra
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);  // Valor predeterminado de 1 si no se encuentra
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);  // Guardar en PlayerPrefs
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);  // Guardar en PlayerPrefs
    }
}