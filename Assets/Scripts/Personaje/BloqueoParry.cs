using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueoParry : MonoBehaviour
{
    private Animator anim;
    private bool bloqueando = false;
    private float tiempoDeBloqueo = 1f; // Duraci�n de la animaci�n de bloqueo (parry)

    void Start()
    {
        // Obtener el Animator para controlar las animaciones
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Activamos el bloqueo con el click derecho
        if (Input.GetMouseButtonDown(1) && !bloqueando) // 1 es el bot�n derecho del rat�n
        {
            StartCoroutine(Bloquear());
        }
    }

    // Coroutine para manejar la animaci�n de bloqueo
    IEnumerator Bloquear()
    {
        bloqueando = true;

        // Activamos el Bool de bloqueo para reproducir la animaci�n de Defenderse (parry)
        anim.SetBool("Bloquear", true);

        // Esperamos a que la animaci�n termine (usamos el tiempo de la animaci�n de bloqueo)
        yield return new WaitForSeconds(tiempoDeBloqueo);

        // Terminamos el bloqueo
        bloqueando = false;

        // Desactivamos el Bool de bloqueo
        anim.SetBool("Bloquear", false);

        // Verificamos si el personaje est� en movimiento o en idle para decidir a qu� animaci�n volver
        if (anim.GetBool("Correr") == false) // Si no est� corriendo
        {
            anim.SetTrigger("Idle");
        }
        else
        {
            anim.SetTrigger("Correr");
        }
    }
}