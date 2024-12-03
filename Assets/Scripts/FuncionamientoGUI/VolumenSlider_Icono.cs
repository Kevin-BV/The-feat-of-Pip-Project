using UnityEngine;
using UnityEngine.UI;

public class VolumenSlider_Icono : MonoBehaviour
{
    public Slider musicSlider;  // Slider para controlar el volumen de la m�sica
    public Slider sfxSlider;    // Slider para controlar el volumen de los efectos de sonido

    public GameObject musicIconObject;  // GameObject que contiene las im�genes de sonido de la m�sica
    public GameObject sfxIconObject;    // GameObject que contiene las im�genes de sonido de los efectos de sonido

    public GameObject musicOnImage;     // Imagen de sonido activado para la m�sica
    public GameObject musicOffImage;    // Imagen de mute (silencio) para la m�sica

    public GameObject sfxOnImage;       // Imagen de sonido activado para los efectos de sonido
    public GameObject sfxOffImage;      // Imagen de mute (silencio) para los efectos de sonido

    public AudioSource musicAudioSource;  // AudioSource que controla la m�sica
    public AudioSource sfxAudioSource;    // AudioSource que controla los efectos de sonido generales

    public AudioSource playerSfxAudioSource; // AudioSource espec�fico para los efectos de sonido del jugador
    public AudioSource rataSfxAudioSource; // AudioSource espec�fico para los efectos de sonido del jugador
    public AudioSource avispaSfxAudioSource; // AudioSource espec�fico para los efectos de sonido del jugador
    public AudioSource bichoSfxAudioSource; // AudioSource espec�fico para los efectos de sonido del jugador


    private void Start()
    {
        // Asegurarnos de que el volumen de los sliders est� sincronizado con los valores actuales
        musicSlider.value = musicAudioSource.volume;
        sfxSlider.value = sfxAudioSource.volume;

        // Asignar los eventos OnValueChanged para cada slider
        musicSlider.onValueChanged.AddListener(HandleMusicVolumeChange);
        sfxSlider.onValueChanged.AddListener(HandleSfxVolumeChange);

        // Establecer las im�genes correctas al inicio
        UpdateMusicIcon(musicAudioSource.volume);
        UpdateSfxIcon(sfxAudioSource.volume);
    }

    // Este m�todo se llamar� cuando el slider de m�sica cambie su valor
    public void HandleMusicVolumeChange(float value)
    {
        // Cambiar el volumen de la m�sica seg�n el valor del slider
        musicAudioSource.volume = value;

        // Actualizar el icono de la m�sica
        UpdateMusicIcon(value);
    }

    // Este m�todo se llamar� cuando el slider de efectos de sonido cambie su valor
    public void HandleSfxVolumeChange(float value)
    {
        // Cambiar el volumen de los efectos de sonido generales seg�n el valor del slider
        sfxAudioSource.volume = value;

        // Cambiar el volumen del jugador seg�n el slider de SFX
        if (playerSfxAudioSource != null)
        {
            playerSfxAudioSource.volume = value;
        }
        // Cambiar el volumen del bicho seg�n el slider de SFX
        if (bichoSfxAudioSource != null)
        {
            bichoSfxAudioSource.volume = value;
        }
        // Cambiar el volumen de la rata seg�n el slider de SFX
        if (rataSfxAudioSource != null)
        {
            rataSfxAudioSource.volume = value;
        }

        // Cambiar el volumen de la avispa seg�n el slider de SFX
        if (avispaSfxAudioSource != null)
        {
            avispaSfxAudioSource.volume = value;
        }

        // Actualizar el icono de los efectos de sonido
        UpdateSfxIcon(value);
    }

    private void UpdateMusicIcon(float volume)
    {
        if (volume == 0)
        {
            // Si el volumen de la m�sica es 0, mostrar la imagen de mute de m�sica
            musicOnImage.SetActive(false);
            musicOffImage.SetActive(true);
        }
        else
        {
            // Si el volumen de la m�sica no es 0, mostrar la imagen de sonido activado de m�sica
            musicOnImage.SetActive(true);
            musicOffImage.SetActive(false);
        }
    }

    private void UpdateSfxIcon(float volume)
    {
        if (volume == 0)
        {
            // Si el volumen de los efectos de sonido es 0, mostrar la imagen de mute de SFX
            sfxOnImage.SetActive(false);
            sfxOffImage.SetActive(true);
        }
        else
        {
            // Si el volumen de los efectos de sonido no es 0, mostrar la imagen de sonido activado de SFX
            sfxOnImage.SetActive(true);
            sfxOffImage.SetActive(false);
        }
    }
}
