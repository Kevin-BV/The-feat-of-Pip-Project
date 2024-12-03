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
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        // Puedes guardar el valor en PlayerPrefs si deseas persistirlo
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
}
