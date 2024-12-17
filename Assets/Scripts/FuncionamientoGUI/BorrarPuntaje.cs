using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Necesario para trabajar con botones


public class BorrarPuntaje : MonoBehaviour
{
    public Button botonBorrar;  // Referencia al bot�n en el inspector

    void Start()
    {
        // Asegurarse de que el bot�n est� configurado correctamente
        if (botonBorrar != null)
        {
            botonBorrar.onClick.AddListener(ResetScore);  // Asignar el m�todo al bot�n
        }
    }

    // M�todo para borrar todos los PlayerPrefs excepto los de sonido
    public void ResetScore()
    {
        // Guardar las configuraciones de sonido antes de borrar los PlayerPrefs
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);

        // Borra todos los PlayerPrefs
        PlayerPrefs.DeleteAll();
        Debug.Log("Todos los PlayerPrefs han sido reiniciados.");

        // Restaurar las configuraciones de sonido
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.Save();
    }
}