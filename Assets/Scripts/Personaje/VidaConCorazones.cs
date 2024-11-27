using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class VidaConCorazones : MonoBehaviour
{
    [Header("Configuraci�n de Vida")]
    public int vidaMaxima = 6; // Vida m�xima (6 puntos de vida = 3 corazones completos)
    private int vidaActual; // Vida actual del jugador

    [Header("Configuraci�n del UI de Corazones")]
    public List<Image> corazones; // Lista de im�genes de los corazones en el Canvas
    public Sprite corazonLleno;  // Sprite de un coraz�n completo
    public Sprite corazonMitad; // Sprite de un coraz�n a la mitad
    public Sprite corazonVacio; // Sprite de un coraz�n vac�o

    void Start()
    {
        // Inicializamos la vida al m�ximo y actualizamos el UI
        vidaActual = vidaMaxima;
        ActualizarCorazones();
    }

    /// <summary>
    /// M�todo para recibir da�o.
    /// </summary>
    /// <param name="dano">Cantidad de da�o recibido.</param>
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
    /// M�todo para curar al jugador.
    /// </summary>
    /// <param name="curacion">Cantidad de puntos de vida a recuperar.</param>
    public void Curar(int curacion)
    {
        vidaActual = Mathf.Min(vidaActual + curacion, vidaMaxima); // Aseguramos que la vida no pase del m�ximo
        ActualizarCorazones();
    }

    /// <summary>
    /// Actualiza el estado visual de los corazones en el UI.
    /// </summary>
    private void ActualizarCorazones()
    {
        for (int i = 0; i < corazones.Count; i++)
        {
            int puntoDeVida = (i + 1) * 2; // Cada coraz�n representa 2 puntos de vida

            if (vidaActual >= puntoDeVida)
            {
                corazones[i].sprite = corazonLleno; // Coraz�n lleno
            }
            else if (vidaActual == puntoDeVida - 1)
            {
                corazones[i].sprite = corazonMitad; // Coraz�n a la mitad
            }
            else
            {
                corazones[i].sprite = corazonVacio; // Coraz�n vac�o
            }
        }
    }

    /// <summary>
    /// L�gica que se ejecuta cuando la vida del jugador llega a 0.
    /// </summary>
    private void Morir()
    {
        Debug.Log("El jugador ha muerto.");
        // Aqu� puedes implementar la l�gica de muerte (reiniciar nivel, mostrar pantalla de game over, etc.)
    }
}