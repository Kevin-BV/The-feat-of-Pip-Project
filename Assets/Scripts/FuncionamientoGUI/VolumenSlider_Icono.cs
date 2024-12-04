using UnityEngine;
using UnityEngine.UI;

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

    public AudioSource playerSfxAudioSource;  // AudioSource para los efectos del jugador
    public AudioSource rataSfxAudioSource;    // AudioSource para los efectos de la rata
    public AudioSource avispaSfxAudioSource;  // AudioSource para los efectos de la avispa
    public AudioSource bichoSfxAudioSource;   // AudioSource para los efectos del bicho
    public AudioSource aranaSfxAudioSource;   // AudioSource para los efectos de la araña

    private void Start()
    {
        // Sincronizar los sliders con los valores actuales
        musicSlider.value = musicAudioSource.volume;
        sfxSlider.value = sfxAudioSource.volume;

        // Asignar eventos de cambio de volumen
        musicSlider.onValueChanged.AddListener(HandleMusicVolumeChange);
        sfxSlider.onValueChanged.AddListener(HandleSfxVolumeChange);

        // Sincronizar los valores iniciales de los SFX
        SincronizarTodosLosSfx();

        // Actualizar íconos
        UpdateMusicIcon(musicAudioSource.volume);
        UpdateSfxIcon(sfxAudioSource.volume);
    }

    public void HandleMusicVolumeChange(float value)
    {
        // Cambiar el volumen de la música
        musicAudioSource.volume = value;

        // Actualizar el ícono
        UpdateMusicIcon(value);
    }

    public void HandleSfxVolumeChange(float value)
    {
        // Cambiar el volumen general de efectos de sonido
        sfxAudioSource.volume = value;

        // Aplicar el cambio a todos los SFX individuales
        if (playerSfxAudioSource != null) playerSfxAudioSource.volume = value;
        if (rataSfxAudioSource != null) rataSfxAudioSource.volume = value;
        if (avispaSfxAudioSource != null) avispaSfxAudioSource.volume = value;
        if (bichoSfxAudioSource != null) bichoSfxAudioSource.volume = value;
        if (aranaSfxAudioSource != null) aranaSfxAudioSource.volume = value;

        // Actualizar el ícono de SFX
        UpdateSfxIcon(value);
    }

    private void SincronizarTodosLosSfx()
    {
        // Sincronizar los volúmenes iniciales de los SFX con el slider
        if (playerSfxAudioSource != null) playerSfxAudioSource.volume = sfxSlider.value;
        if (rataSfxAudioSource != null) rataSfxAudioSource.volume = sfxSlider.value;
        if (avispaSfxAudioSource != null) avispaSfxAudioSource.volume = sfxSlider.value;
        if (bichoSfxAudioSource != null) bichoSfxAudioSource.volume = sfxSlider.value;
        if (aranaSfxAudioSource != null) aranaSfxAudioSource.volume = sfxSlider.value;
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