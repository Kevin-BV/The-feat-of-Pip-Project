using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class VolumenSlider_Icono : MonoBehaviour
{
    public Slider musicSlider;  // Slider para controlar el volumen de la m�sica
    public Slider sfxSlider;    // Slider para controlar el volumen de los efectos de sonido

    public GameObject musicOnImage;     // Imagen de sonido activado para la m�sica
    public GameObject musicOffImage;    // Imagen de mute para la m�sica

    public GameObject sfxOnImage;       // Imagen de sonido activado para los efectos de sonido
    public GameObject sfxOffImage;      // Imagen de mute para los efectos de sonido

    public AudioSource musicAudioSource;  // AudioSource que controla la m�sica
    public AudioSource sfxAudioSource;    // AudioSource general de efectos de sonido

    public AudioSource playerSfxAudioSource;  // AudioSource para los efectos del jugador
    public AudioSource rataSfxAudioSource;    // AudioSource para los efectos de la rata
    public AudioSource avispaSfxAudioSource;  // AudioSource para los efectos de la avispa
    public AudioSource bichoSfxAudioSource;   // AudioSource para los efectos del bicho
    public AudioSource aranaSfxAudioSource;   // AudioSource para los efectos de la ara�a
    public AudioSource aranitaSfxAudioSource;   // AudioSource para los efectos de la ara�ita
    public AudioSource HumoParrillasAudioSource;   // AudioSource para los efectos de la parrila
    public AudioSource cajaDeFosforosAudioSource;   // AudioSource para los efectos de la ara�ita
    public AudioSource gusanoPuntajeAudioSource;   // AudioSource para los efectos de la ara�ita




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

        // Actualizar �conos
        UpdateMusicIcon(musicAudioSource.volume);
        UpdateSfxIcon(sfxAudioSource.volume);
    }

    public void HandleMusicVolumeChange(float value)
    {
        // Cambiar el volumen de la m�sica
        musicAudioSource.volume = value;

        // Actualizar el �cono
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
        if (aranitaSfxAudioSource != null) aranitaSfxAudioSource.volume = value;
        if (HumoParrillasAudioSource != null) HumoParrillasAudioSource.volume = value;
        if (cajaDeFosforosAudioSource != null) cajaDeFosforosAudioSource.volume = value;
        if (gusanoPuntajeAudioSource != null) gusanoPuntajeAudioSource.volume = value;



        // Actualizar el �cono de SFX
        UpdateSfxIcon(value);
    }

    private void SincronizarTodosLosSfx()
    {
        // Sincronizar los vol�menes iniciales de los SFX con el slider
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