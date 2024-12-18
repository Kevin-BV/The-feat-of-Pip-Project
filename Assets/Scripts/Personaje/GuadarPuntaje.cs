using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardarPuntaje : MonoBehaviour
{
    private int puntajeGuardado = 0;

    private void Start()
    {
        // Guardar el puntaje actual al empezar un nuevo nivel
        GuardarPuntajeActual();
    }

    // Método para guardar el puntaje cuando el personaje se mueve o inicia el nivel
    public void GuardarPuntajeActual()
    {
        // Obtener el puntaje actual desde PlayerPrefs (si existe, sino inicializar a 0)
        puntajeGuardado = PlayerPrefs.GetInt("Puntaje", 0);

        // Guardar el puntaje como "PuntajeInicial" para poder usarlo en el siguiente nivel
        PlayerPrefs.SetInt("PuntajeInicial", puntajeGuardado);
    }

    // Este método se puede usar cuando se reintenta o se cambia de nivel
    public void ActualizarPuntaje()
    {
        // Obtener el puntaje del Puntaje.cs para actualizar
        int puntajeActual = PlayerPrefs.GetInt("Puntaje", 0);

        // Actualizar el puntaje guardado para la siguiente escena
        PlayerPrefs.SetInt("PuntajeInicial", puntajeActual);
    }
}