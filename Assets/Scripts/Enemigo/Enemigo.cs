using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public int salud = 4; // Vida del enemigo (4 de vida)
    public Transform puntoDeDa�o; // El transform vac�o que definir� el �rea de da�o
    public float tama�oDelGizmo = 1f; // Tama�o del �rea del Gizmo (lo podr�s modificar desde el inspector)

    private Animator anim;
    private Rigidbody rb; // Componente Rigidbody para bloquear el movimiento
    private MovimientoEnemy scriptMovimiento; // Referencia al script de movimiento del enemigo

    void Start()
    {
        // Obtener el componente Animator para poder controlar las animaciones
        anim = GetComponent<Animator>();
        // Obtener el componente Rigidbody
        rb = GetComponent<Rigidbody>(); // Usa Rigidbody2D si es un 2D

        // Obtener el script de movimiento del enemigo
        scriptMovimiento = GetComponent<MovimientoEnemy>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Verificamos si el objeto que entr� en contacto tiene el tag "Player" o cualquier otra condici�n que defina tu ataque
        if (other.CompareTag("Player"))
        {
            Debug.Log("�El enemigo ha sido tocado por el jugador!");
            // Aplicar da�o
            TomarDa�o(1); // Este es un ejemplo, ajusta la cantidad de da�o si es necesario
        }
    }

    public void TomarDa�o(int cantidad)
    {
        // Evitar que el enemigo ataque mientras est� en la animaci�n de da�o
        anim.SetBool("isTakingDamage", true);

        // Desactivar el movimiento del enemigo mientras recibe da�o
        scriptMovimiento.enabled = false;

        salud -= cantidad;

        if (salud <= 0)
        {
            Morir();
        }

        // Reiniciar el par�metro "isTakingDamage" y habilitar el movimiento despu�s de un tiempo
        StartCoroutine(ReiniciarDa�o());
    }

    private IEnumerator ReiniciarDa�o()
    {
        // Esperar el tiempo que dure la animaci�n de da�o
        yield return new WaitForSeconds(0.5f); // Cambia este valor seg�n la duraci�n de tu animaci�n de da�o

        // Activar la animaci�n de da�o terminada
        anim.SetBool("isTakingDamage", false);

        // Habilitar el movimiento despu�s de la animaci�n de da�o
        scriptMovimiento.enabled = true;
    }

    void Morir()
    {
        // Reproducir la animaci�n de muerte
        anim.SetTrigger("Death");

        // Bloquear el movimiento del enemigo (si tiene Rigidbody)
        if (rb != null)
        {
            rb.isKinematic = true; // Esto desactiva la f�sica del Rigidbody, evitando que se mueva
        }

        // Desactivar el script de movimiento (si existe)
        if (scriptMovimiento != null)
        {
            scriptMovimiento.enabled = false; // Desactiva el script de movimiento
        }

        // Destruir al enemigo despu�s de 5 segundos
        Destroy(gameObject, 5f); // El enemigo se destruye 5 segundos despu�s de que se active la animaci�n
    }

    private void OnDrawGizmosSelected()
    {
        if (puntoDeDa�o != null)
        {
            Gizmos.color = Color.red; // Color rojo para el gizmo de da�o
            Gizmos.DrawWireSphere(puntoDeDa�o.position, tama�oDelGizmo); // Dibujamos un c�rculo donde el enemigo recibe da�o
        }
    }
}