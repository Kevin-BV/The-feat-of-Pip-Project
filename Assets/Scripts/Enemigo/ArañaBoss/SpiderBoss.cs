using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBoss : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento
    public float moveAmount = 7f; // Cantidad de movimiento
    public float waitTime = 10f; // Tiempo de espera en segundos al principio y entre ciclos
    public Transform player; // Referencia al jugador (arrastrar el objeto del jugador al inspector)
    public float gizmoSize = 1f; // Tamaño del Gizmo de dirección

    private Vector3 startingPosition; // Posición inicial de la araña

    void Start()
    {
        startingPosition = transform.position; // Guardar la posición inicial
        StartCoroutine(MoveSpider()); // Iniciar la secuencia de movimiento
    }

    void Update()
    {
        // Hacer que la araña mire al jugador cambiando su escala en el eje X
        if (player != null)
        {
            // Calcular la dirección hacia el jugador
            Vector3 directionToPlayer = player.position - transform.position;

            // Si el jugador está a la izquierda de la araña, invertir la escala X
            if (directionToPlayer.x < 0)
            {
                // Cambiar la escala en el eje X para "mirar" hacia la izquierda
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                // Cambiar la escala en el eje X para "mirar" hacia la derecha
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    IEnumerator MoveSpider()
    {
        // Esperar 10 segundos al principio antes de comenzar el movimiento
        yield return new WaitForSeconds(waitTime);

        while (true) // El ciclo continuará indefinidamente
        {
            // Subir 7 espacios en Y
            Vector3 targetPosition = startingPosition + new Vector3(0, moveAmount, 0);
            yield return StartCoroutine(MoveToPosition(targetPosition));

            // Mover a la izquierda 7 espacios en X
            targetPosition = transform.position + new Vector3(-moveAmount, 0, 0);
            yield return StartCoroutine(MoveToPosition(targetPosition));

            // Bajar 7 espacios en Y
            targetPosition = transform.position + new Vector3(0, -moveAmount, 0);
            yield return StartCoroutine(MoveToPosition(targetPosition));

            // Esperar 10 segundos después de completar un ciclo de movimiento
            yield return new WaitForSeconds(waitTime);

            // Subir 7 espacios en Y nuevamente
            targetPosition = transform.position + new Vector3(0, moveAmount, 0);
            yield return StartCoroutine(MoveToPosition(targetPosition));

            // Avanzar 10 espacios a la derecha en X
            targetPosition = transform.position + new Vector3(10, 0, 0);
            yield return StartCoroutine(MoveToPosition(targetPosition));

            // Bajar 7 espacios en Y
            targetPosition = transform.position + new Vector3(0, -moveAmount, 0);
            yield return StartCoroutine(MoveToPosition(targetPosition));

            // Esperar 10 segundos después de completar el ciclo de movimiento hacia el otro lado
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        float timeToReach = Vector3.Distance(transform.position, targetPosition) / moveSpeed;
        float elapsedTime = 0;

        while (elapsedTime < timeToReach)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, (elapsedTime / timeToReach));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition; // Asegurarse de llegar al punto final
    }

    // Dibujar el Gizmo en el editor para ver la dirección
    void OnDrawGizmos()
    {
        if (player != null)
        {
            // Dibujar una línea que muestra la dirección hacia el jugador
            Gizmos.color = Color.red; // Color de la línea
            Gizmos.DrawLine(transform.position, player.position);
            Gizmos.DrawSphere(transform.position + (player.position - transform.position).normalized * gizmoSize, gizmoSize);
        }
    }
}