using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class VidaConCorazones : MonoBehaviour
{
    [Header("Configuración de Vida")]
    public int vidaMaxima = 6; // Vida máxima (6 puntos de vida = 3 corazones completos)
    private int vidaActual; // Vida actual del jugador

    [Header("Configuración del UI de Corazones")]
    public List<Image> corazones; // Lista de imágenes de los corazones en el Canvas
    public Sprite corazonLleno;  // Sprite de un corazón completo
    public Sprite corazonMitad; // Sprite de un corazón a la mitad
    public Sprite corazonVacio; // Sprite de un corazón vacío

    void Start()
    {
        // Inicializamos la vida al máximo y actualizamos el UI
        vidaActual = vidaMaxima;
        ActualizarCorazones();
    }

    /// <summary>
    /// Método para recibir daño.
    /// </summary>
    /// <param name="dano">Cantidad de daño recibido.</param>
    public void RecibirDano(int dano)
    {
        vidaActual = Mathf.Max(vidaActual - dano, 0); // Aseguramos que la vida no sea menor a 0
        ActualizarCorazones();

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    /// <summary>
    /// Método para curar al jugador.
    /// </summary>
    /// <param name="curacion">Cantidad de puntos de vida a recuperar.</param>
    public void Curar(int curacion)
    {
        vidaActual = Mathf.Min(vidaActual + curacion, vidaMaxima); // Aseguramos que la vida no pase del máximo
        ActualizarCorazones();
    }

    /// <summary>
    /// Actualiza el estado visual de los corazones en el UI.
    /// </summary>
    private void ActualizarCorazones()
    {
        for (int i = 0; i < corazones.Count; i++)
        {
            int puntoDeVida = (i + 1) * 2; // Cada corazón representa 2 puntos de vida

            if (vidaActual >= puntoDeVida)
            {
                corazones[i].sprite = corazonLleno; // Corazón lleno
            }
            else if (vidaActual == puntoDeVida - 1)
            {
                corazones[i].sprite = corazonMitad; // Corazón a la mitad
            }
            else
            {
                corazones[i].sprite = corazonVacio; // Corazón vacío
            }
        }
    }

    /// <summary>
    /// Lógica que se ejecuta cuando la vida del jugador llega a 0.
    /// </summary>
    private void Morir()
    {
        Debug.Log("El jugador ha muerto.");
        // Aquí puedes implementar la lógica de muerte (reiniciar nivel, mostrar pantalla de game over, etc.)
    }
}