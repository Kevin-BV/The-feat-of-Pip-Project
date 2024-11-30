using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBoss : MonoBehaviour
{
    public float velocidad = 1.0f; // Velocidad de movimiento
    public Vector3 posicionInicial; // Posición inicial de la araña

    private void Start()
    {
        // Guardar la posición inicial
        posicionInicial = transform.position;

        // Iniciar el comportamiento
        StartCoroutine(Movimiento());
    }

    private IEnumerator Movimiento()
    {
        // Esperar 10 segundos
        yield return new WaitForSeconds(10.0f);

        // Mover hacia arriba
        float posY = transform.position.y + 6.0f;
        while (transform.position.y < posY)
        {
            transform.position = new Vector3(transform.position.x, Mathf.MoveTowards(transform.position.y, posY, velocidad * Time.deltaTime), transform.position.z);
            yield return null;
        }

        // Mover hacia la izquierda
        float posX = transform.position.x - 10.0f;
        while (transform.position.x > posX)
        {
            transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, posX, velocidad * Time.deltaTime), transform.position.y, transform.position.z);
            yield return null;
        }

        // Mover hacia abajo
        posY = transform.position.y - 6.0f;
        while (transform.position.y > posY)
        {
            transform.position = new Vector3(transform.position.x, Mathf.MoveTowards(transform.position.y, posY, velocidad * Time.deltaTime), transform.position.z);
            yield return null;
        }

        // Esperar 10 segundos
        yield return new WaitForSeconds(10.0f);

        // Mover hacia arriba para regresar a la posición inicial
        posY = posicionInicial.y;
        while (transform.position.y < posY)
        {
            transform.position = new Vector3(transform.position.x, Mathf.MoveTowards(transform.position.y, posY, velocidad * Time.deltaTime), transform.position.z);
            yield return null;
        }

        // Reiniciar el comportamiento
        StartCoroutine(Movimiento());
    }
}
