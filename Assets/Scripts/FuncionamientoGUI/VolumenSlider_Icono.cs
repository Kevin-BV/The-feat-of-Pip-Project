using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class VolumenSlider_Icono : MonoBehaviour
{
    public Slider musicSlider;  // Slider para controlar el volumen de la música
    public Slider sfxSlider;    // Slider para controlar el volumen de los efectos de sonido

    public GameObject musicOnImage;     // Imagen de sonido activado para la música
    public GameObject musicOffImage;    // Imagen de mute para la música

    public GameObject sfxOnImage;       // Imagen de sonido activado para los efectos de sonido
    public GameObject sfxOffImage;      // Imagen de mute para los efectos de sonido

    public AudioSource musicAudioSource;  // AudioSource que controla la música
    public AudioSource sfxAudioSource;    // AudioSource general de efectos de sonido

    // Otros AudioSources individuales
    public AudioSource playerSfxAudioSource;
    public AudioSource rataSfxAudioSource;
    public AudioSource avispaSfxAudioSource;
    public AudioSource bichoSfxAudioSource;
    public AudioSource aranaSfxAudioSource;
    public AudioSource aranitaSfxAudioSource;
    public AudioSource HumoParrillasAudioSource;
    public AudioSource cajaDeFosforosAudioSource;
    public AudioSource gusanoPuntajeAudioSource;

    private void Start()
    {
        // Cargar los valores guardados o usar valores predeterminados
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float savedSfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // Sincronizar sliders y AudioSources con los valores guardados
        musicSlider.value = savedMusicVolume;
        sfxSlider.value = savedSfxVolume;

        musicAudioSource.volume = savedMusicVolume;
        sfxAudioSource.volume = savedSfxVolume;

        SincronizarTodosLosSfx();

        // Asignar eventos de cambio
        musicSlider.onValueChanged.AddListener(HandleMusicVolumeChange);
        sfxSlider.onValueChanged.AddListener(HandleSfxVolumeChange);

        // Actualizar íconos
        UpdateMusicIcon(savedMusicVolume);
        UpdateSfxIcon(savedSfxVolume);
    }

    public void HandleMusicVolumeChange(float value)
    {
        // Cambiar el volumen de la música y guardar en PlayerPrefs
        musicAudioSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();

        // Actualizar ícono
        UpdateMusicIcon(value);
    }

    public void HandleSfxVolumeChange(float value)
    {
        // Cambiar el volumen general de efectos de sonido y guardar en PlayerPrefs
        sfxAudioSource.volume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();

        // Aplicar el cambio a todos los SFX individuales
        SincronizarTodosLosSfx();

        // Actualizar ícono
        UpdateSfxIcon(value);
    }

    private void SincronizarTodosLosSfx()
    {
        // Sincronizar los volúmenes de los SFX individuales con el slider
        if (playerSfxAudioSource != null) playerSfxAudioSource.volume = sfxSlider.value;
        if (rataSfxAudioSource != null) rataSfxAudioSource.volume = sfxSlider.value;
        if (avispaSfxAudioSource != null) avispaSfxAudioSource.volume = sfxSlider.value;
        if (bichoSfxAudioSource != null) bichoSfxAudioSource.volume = sfxSlider.value;
        if (aranaSfxAudioSource != null) aranaSfxAudioSource.volume = sfxSlider.value;
        if (aranitaSfxAudioSource != null) aranitaSfxAudioSource.volume = sfxSlider.value;
        if (HumoParrillasAudioSource != null) HumoParrillasAudioSource.volume = sfxSlider.value;
        if (cajaDeFosforosAudioSource != null) cajaDeFosforosAudioSource.volume = sfxSlider.value;
        if (gusanoPuntajeAudioSource != null) gusanoPuntajeAudioSource.volume = sfxSlider.value;
    }

    private void UpdateMusicIcon(float volume)
    {
        musicOnImage.SetActive(volume > 0);
        musicOffImage.SetActive(volume == 0);
    }

    private void UpdateSfxIcon(float volume)
    {
        sfxOnImage.SetActive(volume > 0);
        sfxOffImage.SetActive(volume == 0);
    }
}