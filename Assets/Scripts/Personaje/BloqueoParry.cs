using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueoParry : MonoBehaviour
{
    private Animator anim;
    private bool bloqueando = false;
    private bool puedeRecibirDaño = true; // Si el jugador puede recibir daño
    private float tiempoDeBloqueo = 1f; // Duración del bloqueo (1 segundo)
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

    // Coroutine para manejar la animación de bloqueo
    IEnumerator Bloquear()
    {
        bloqueando = true;
        puedeRecibirDaño = false; // El jugador no puede recibir daño durante el bloqueo

        // Activamos la animación de defenderse (parry)
        anim.SetTrigger("Defenderse");

        // Registramos el tiempo en que se hizo el último bloqueo
        tiempoDelUltimoBloqueo = Time.time;

        // Esperamos a que pase el tiempo de bloqueo (1 segundo)
        yield return new WaitForSeconds(tiempoDeBloqueo);

        // Terminamos el bloqueo
        bloqueando = false;
        puedeRecibirDaño = true; // El jugador puede recibir daño nuevamente

        // Volvemos a la animación de idle o correr dependiendo del estado
        if (anim.GetBool("Correr") == false) // Si no está corriendo
        {
            anim.SetTrigger("Idle");
        }
        else
        {
            anim.SetTrigger("Correr");
        }
    }

    // Método que se puede llamar desde otro script para verificar si el jugador puede recibir daño
    public bool PuedeRecibirDaño()
    {
        return puedeRecibirDaño;
    }
}