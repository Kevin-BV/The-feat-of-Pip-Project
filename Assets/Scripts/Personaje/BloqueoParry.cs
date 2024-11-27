using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueoParry : MonoBehaviour
{
    private Animator anim;
    private bool bloqueando = false;
    private bool puedeRecibirDa�o = true; // Si el jugador puede recibir da�o
    private float tiempoDeBloqueo = 1f; // Duraci�n del bloqueo (1 segundo)
    private float tiempoEntreBloqueos = 1f; // Cooldown de bloqueo
    private float tiempoDelUltimoBloqueo = 0f; // Para gestionar el cooldown

    void Start()
    {
        // Obtener el Animator para controlar las animaciones
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Activamos el bloqueo al presionar la tecla C, solo si ha pasado el cooldown
        if (Input.GetKeyDown(KeyCode.C) && Time.time >= tiempoDelUltimoBloqueo + tiempoEntreBloqueos)
        {
            StartCoroutine(Bloquear());
        }
    }

    // Coroutine para manejar la animaci�n de bloqueo
    IEnumerator Bloquear()
    {
        bloqueando = true;
        puedeRecibirDa�o = false; // El jugador no puede recibir da�o durante el bloqueo

        // Activamos la animaci�n de defenderse (parry)
        anim.SetTrigger("Defenderse");

        // Registramos el tiempo en que se hizo el �ltimo bloqueo
        tiempoDelUltimoBloqueo = Time.time;

        // Esperamos a que pase el tiempo de bloqueo (1 segundo)
        yield return new WaitForSeconds(tiempoDeBloqueo);

        // Terminamos el bloqueo
        bloqueando = false;
        puedeRecibirDa�o = true; // El jugador puede recibir da�o nuevamente

        // Volvemos a la animaci�n de idle o correr dependiendo del estado
        if (anim.GetBool("Correr") == false) // Si no est� corriendo
        {
            anim.SetTrigger("Idle");
        }
        else
        {
            anim.SetTrigger("Correr");
        }
    }

    // M�todo que se puede llamar desde otro script para verificar si el jugador puede recibir da�o
    public bool PuedeRecibirDa�o()
    {
        return puedeRecibirDa�o;
    }
}