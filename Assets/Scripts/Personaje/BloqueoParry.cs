using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueoParry : MonoBehaviour
{
    private Animator anim; // Referencia al Animator
    private bool bloqueando = false; // Indica si el jugador est� bloqueando
    private bool puedeRecibirDano = true; // Controla si el jugador puede recibir da�o

    public float tiempoDeBloqueo = 1f; // Duraci�n del bloqueo en segundos

    void Start()
    {
        // Obtener el Animator para controlar las animaciones
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Detectamos si se presiona la tecla "C" y no est� bloqueando
        if (Input.GetMouseButtonDown(1) && !bloqueando)
        {
            Bloquear();
        }
    }

    /// <summary>
    /// Ejecuta el bloqueo y activa la animaci�n "Defenderse".
    /// </summary>
    private void Bloquear()
    {
        bloqueando = true; // Indica que el jugador est� bloqueando
        puedeRecibirDano = false; // Evita que el jugador reciba da�o

        // Activar la animaci�n de "Defenderse"
        anim.SetTrigger("Defenderse");

        // Inicia la l�gica de terminar el bloqueo despu�s del tiempo configurado
        StartCoroutine(DesactivarBloqueo());
    }

    /// <summary>
    /// Corrutina que desactiva el bloqueo tras un tiempo.
    /// </summary>
    IEnumerator DesactivarBloqueo()
    {
        yield return new WaitForSeconds(tiempoDeBloqueo);

        bloqueando = false; // Permite volver a bloquear
        puedeRecibirDano = true; // Vuelve a permitir recibir da�o
    }

    /// <summary>
    /// Verifica si el jugador puede recibir da�o.
    /// </summary>
    public bool PuedeRecibirDano()
    {
        return puedeRecibirDano;
    }
}