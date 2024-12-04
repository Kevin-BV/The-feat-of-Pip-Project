using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueoParry : MonoBehaviour
{
    private Animator anim; // Referencia al Animator
    private bool bloqueando = false; // Indica si el jugador está bloqueando
    private bool puedeRecibirDano = true; // Controla si el jugador puede recibir daño

    [Header("Configuración de Bloqueo")]
    public float tiempoDeBloqueo = 1f; // Duración del bloqueo en segundos
    public float tiempoInvulnerabilidad = 0.5f; // Tiempo de invulnerabilidad después de bloquear (editable desde el inspector)

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
        // Detectamos si se presiona el clic derecho y no está bloqueando
        if (Input.GetMouseButtonDown(1) && !bloqueando)
        {
            Bloquear();
        }
    }

    /// <summary>
    /// Ejecuta el bloqueo y activa la animación "Defenderse".
    /// </summary>
    private void Bloquear()
    {
        bloqueando = true; // Indica que el jugador está bloqueando
        puedeRecibirDano = false; // Evita que el jugador reciba daño
        audioSource.PlayOneShot(escudo);

        // Activar la animación de "Defenderse"
        anim.SetTrigger("Defenderse");

        // Inicia la lógica de terminar el bloqueo después del tiempo configurado
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
    /// Corrutina que activa la invulnerabilidad del jugador después de bloquear.
    /// </summary>
    IEnumerator ActivarInvulnerabilidad()
    {
        yield return new WaitForSeconds(tiempoInvulnerabilidad);

        puedeRecibirDano = true; // Vuelve a permitir recibir daño después del tiempo de invulnerabilidad
    }

    /// <summary>
    /// Verifica si el jugador puede recibir daño.
    /// </summary>
    public bool PuedeRecibirDano()
    {
        return puedeRecibirDano;
    }
}