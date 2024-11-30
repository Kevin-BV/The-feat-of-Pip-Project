using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBoss : MonoBehaviour
{
    [Header("Configuración del Movimiento")]
    public float distanciaX = 10f; // Distancia en X (izquierda y derecha)
    public float distanciaY = 7f; // Distancia en Y (arriba y abajo)
    public float velocidadDeMovimiento = 2f; // Velocidad en la que se mueve en Y y X
    public float tiempoDeEspera = 10f; // Tiempo de espera entre movimientos

    private Vector3 posicionInicial;
    private bool enMovimiento = false;
    private bool moviendoALaIzquierda = false; // Determina si se mueve a la izquierda o a la derecha

    void Start()
    {
        posicionInicial = transform.position; // Guardamos la posición inicial de la araña
        StartCoroutine(CicloMovimiento()); // Iniciamos el ciclo de movimiento
    }

    void Update()
    {
        if (enMovimiento)
        {
            // Movimiento en Y (subir o bajar)
            float objetivoY = moviendoALaIzquierda ? posicionInicial.y + distanciaY : posicionInicial.y - distanciaY;
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(transform.position.x, objetivoY, transform.position.z),
                velocidadDeMovimiento * Time.deltaTime);

            // Verificamos si ha alcanzado la altura máxima o mínima en Y
            if (transform.position.y >= posicionInicial.y + distanciaY && moviendoALaIzquierda)
            {
                moviendoALaIzquierda = false; // Comienza a bajar
                StartCoroutine(MoverEnX()); // Comienza a mover a la izquierda o derecha
            }
            else if (transform.position.y <= posicionInicial.y - distanciaY && !moviendoALaIzquierda)
            {
                moviendoALaIzquierda = true; // Comienza a subir
                StartCoroutine(MoverEnX()); // Comienza a mover a la izquierda o derecha
            }
        }
    }

    IEnumerator CicloMovimiento()
    {
        while (true)
        {
            // La araña se mantiene en su posición durante el tiempo de espera
            yield return new WaitForSeconds(tiempoDeEspera);

            // Iniciamos el movimiento hacia arriba o abajo
            enMovimiento = true;
        }
    }

    IEnumerator MoverEnX()
    {
        // Esperamos un poco antes de mover en X
        yield return new WaitForSeconds(1f);

        // Realizamos el movimiento en dirección X (izquierda o derecha)
        float direccionX = moviendoALaIzquierda ? -distanciaX : distanciaX;
        transform.position = new Vector3(transform.position.x + direccionX, transform.position.y, transform.position.z);

        // Cambiar la dirección de la rotación cuando llega a la izquierda o derecha
        if (moviendoALaIzquierda)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Mira hacia la izquierda
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1); // Mira hacia la derecha
        }

        // Invertimos la dirección para el siguiente movimiento en X
        moviendoALaIzquierda = !moviendoALaIzquierda;

        // Después de moverse, esperamos un poco antes de continuar el ciclo
        yield return new WaitForSeconds(tiempoDeEspera);

        // Reiniciamos el ciclo de subida y bajada
        enMovimiento = false;
        posicionInicial = transform.position; // Actualizamos la posición inicial para el siguiente movimiento
    }

    // Dibujar un Gizmo para visualizar la distancia de teletransporte
    private void OnDrawGizmosSelected()
    {
        // Establecemos el color del Gizmo
        Gizmos.color = Color.green;

        // Dibujamos una línea que representa la distancia de teletransporte en el eje X
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + distanciaX, transform.position.y, transform.position.z));
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x - distanciaX, transform.position.y, transform.position.z));

        // También podemos dibujar un Gizmo para la distancia de subida/bajada (en Y)
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y + distanciaY, transform.position.z));
    }
}