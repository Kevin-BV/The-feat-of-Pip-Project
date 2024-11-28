using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public int salud = 4; // Vida del enemigo (4 de vida)
    public Transform puntoDeDaño; // El transform vacío que definirá el área de daño
    public float tamañoDelGizmo = 1f; // Tamaño del área del Gizmo (lo podrás modificar desde el inspector)

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
        // Verificamos si el objeto que entró en contacto tiene el tag "Player" o cualquier otra condición que defina tu ataque
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡El enemigo ha sido tocado por el jugador!");
            // Aplicar daño
            TomarDaño(1); // Este es un ejemplo, ajusta la cantidad de daño si es necesario
        }
    }

    public void TomarDaño(int cantidad)
    {
        // Evitar que el enemigo ataque mientras está en la animación de daño
        anim.SetBool("isTakingDamage", true);

        // Desactivar el movimiento del enemigo mientras recibe daño
        scriptMovimiento.enabled = false;

        salud -= cantidad;

        if (salud <= 0)
        {
            Morir();
        }

        // Reiniciar el parámetro "isTakingDamage" y habilitar el movimiento después de un tiempo
        StartCoroutine(ReiniciarDaño());
    }

    private IEnumerator ReiniciarDaño()
    {
        // Esperar el tiempo que dure la animación de daño
        yield return new WaitForSeconds(0.5f); // Cambia este valor según la duración de tu animación de daño

        // Activar la animación de daño terminada
        anim.SetBool("isTakingDamage", false);

        // Habilitar el movimiento después de la animación de daño
        scriptMovimiento.enabled = true;
    }

    void Morir()
    {
        // Reproducir la animación de muerte
        anim.SetTrigger("Death");

        // Bloquear el movimiento del enemigo (si tiene Rigidbody)
        if (rb != null)
        {
            rb.isKinematic = true; // Esto desactiva la física del Rigidbody, evitando que se mueva
        }

        // Desactivar el script de movimiento (si existe)
        if (scriptMovimiento != null)
        {
            scriptMovimiento.enabled = false; // Desactiva el script de movimiento
        }

        // Destruir al enemigo después de 5 segundos
        Destroy(gameObject, 5f); // El enemigo se destruye 5 segundos después de que se active la animación
    }

    private void OnDrawGizmosSelected()
    {
        if (puntoDeDaño != null)
        {
            Gizmos.color = Color.red; // Color rojo para el gizmo de daño
            Gizmos.DrawWireSphere(puntoDeDaño.position, tamañoDelGizmo); // Dibujamos un círculo donde el enemigo recibe daño
        }
    }
}