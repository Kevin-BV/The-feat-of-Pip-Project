using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueoParry : MonoBehaviour
{
    private Animator anim;
    private bool bloqueando = false;
    private float tiempoDeBloqueo = 1f; // Duración de la animación de bloqueo (parry)

    void Start()
    {
        // Obtener el Animator para controlar las animaciones
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Activamos el bloqueo con el click derecho
        if (Input.GetMouseButtonDown(1) && !bloqueando) // 1 es el botón derecho del ratón
        {
            StartCoroutine(Bloquear());
        }
    }

    // Coroutine para manejar la animación de bloqueo
    IEnumerator Bloquear()
    {
        bloqueando = true;

        // Activamos el Bool de bloqueo para reproducir la animación de Defenderse (parry)
        anim.SetBool("Bloquear", true);

        // Esperamos a que la animación termine (usamos el tiempo de la animación de bloqueo)
        yield return new WaitForSeconds(tiempoDeBloqueo);

        // Terminamos el bloqueo
        bloqueando = false;

        // Desactivamos el Bool de bloqueo
        anim.SetBool("Bloquear", false);

        // Verificamos si el personaje está en movimiento o en idle para decidir a qué animación volver
        if (anim.GetBool("Correr") == false) // Si no está corriendo
        {
            anim.SetTrigger("Idle");
        }
        else
        {
            anim.SetTrigger("Correr");
        }
    }
}