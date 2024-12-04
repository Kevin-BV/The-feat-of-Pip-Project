using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueoParry : MonoBehaviour
{
    private Animator anim; // Referencia al Animator
    private bool bloqueando = false; // Indica si el jugador est� bloqueando
    private bool puedeRecibirDano = true; // Controla si el jugador puede recibir da�o

    [Header("Configuraci�n de Bloqueo")]
    public float tiempoDeBloqueo = 1f; // Duraci�n del bloqueo en segundos
    public float tiempoInvulnerabilidad = 0.5f; // Tiempo de invulnerabilidad despu�s de bloquear (editable desde el inspector)

    public AudioClip escudo;
    private AudioSource audioSource;

    void Start()
    {
        // Obtener el Animator para controlar las animaciones
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Detectamos si se presiona el clic derecho y no est� bloqueando
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
        audioSource.PlayOneShot(escudo);

        // Activar la animaci�n de "Defenderse"
        anim.SetTrigger("Defenderse");

        // Inicia la l�gica de terminar el bloqueo despu�s del tiempo configurado
        StartCoroutine(DesactivarBloqueo());
        StartCoroutine(ActivarInvulnerabilidad());
    }

    /// <summary>
    /// Corrutina que desactiva el bloqueo tras un tiempo.
    /// </summary>
    IEnumerator DesactivarBloqueo()
    {
        yield return new WaitForSeconds(tiempoDeBloqueo);

        bloqueando = false; // Permite volver a bloquear
    }

    /// <summary>
    /// Corrutina que activa la invulnerabilidad del jugador despu�s de bloquear.
    /// </summary>
    IEnumerator ActivarInvulnerabilidad()
    {
        yield return new WaitForSeconds(tiempoInvulnerabilidad);

        puedeRecibirDano = true; // Vuelve a permitir recibir da�o despu�s del tiempo de invulnerabilidad
    }

    /// <summary>
    /// Verifica si el jugador puede recibir da�o.
    /// </summary>
    public bool PuedeRecibirDano()
    {
        return puedeRecibirDano;
    }
}