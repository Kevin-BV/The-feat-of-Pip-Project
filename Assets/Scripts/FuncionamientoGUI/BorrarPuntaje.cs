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

    // M�todo para borrar todos los PlayerPrefs
    public void ResetScore()
    {
        // Borra todos los PlayerPrefs
        PlayerPrefs.DeleteAll();
        Debug.Log("Todos los PlayerPrefs han sido reiniciados.");
    }
}