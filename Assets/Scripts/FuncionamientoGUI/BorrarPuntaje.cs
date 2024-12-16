using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Necesario para trabajar con botones

public class BorrarPuntaje : MonoBehaviour
{
    public Button botonBorrar;  // Referencia al botón en el inspector

    void Start()
    {
        // Asegurarse de que el botón esté configurado correctamente
        if (botonBorrar != null)
        {
            botonBorrar.onClick.AddListener(ResetScore);  // Asignar el método al botón
        }
    }

    // Método para borrar todos los PlayerPrefs
    public void ResetScore()
    {
        // Borra todos los PlayerPrefs
        PlayerPrefs.DeleteAll();
        Debug.Log("Todos los PlayerPrefs han sido reiniciados.");
    }
}