using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorrarPuntaje : MonoBehaviour
{
    // Método para borrar el puntaje
    public void ResetScore()
    {
        // Borra el puntaje almacenado en PlayerPrefs
        PlayerPrefs.DeleteKey("Puntaje");
        Debug.Log("El puntaje ha sido reiniciado.");
    }
}
