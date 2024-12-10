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

    // Referencia al script de ataque
    public AtaquePersonaje scriptAtaque;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Buscar el script de ataque en el mismo objeto si no está asignado
        if (scriptAtaque == null)
        {
            scriptAtaque = GetComponent<AtaquePersonaje>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !bloqueando)
        {
            Bloquear();
        }
    }

    private void Bloquear()
    {
        bloqueando = true;
        puedeRecibirDano = false;
        audioSource.PlayOneShot(escudo);
        anim.SetTrigger("Defenderse");

        // Desactivar el ataque
        if (scriptAtaque != null)
        {
            scriptAtaque.enabled = false;
        }

        StartCoroutine(DesactivarBloqueo());
        StartCoroutine(ActivarInvulnerabilidad());
    }

    IEnumerator DesactivarBloqueo()
    {
        yield return new WaitForSeconds(tiempoDeBloqueo);

        bloqueando = false;

        // Reactivar el ataque
        if (scriptAtaque != null)
        {
            scriptAtaque.enabled = true;
        }
    }

    IEnumerator ActivarInvulnerabilidad()
    {
        yield return new WaitForSeconds(tiempoInvulnerabilidad);
        puedeRecibirDano = true;
    }

    public bool PuedeRecibirDano()
    {
        return puedeRecibirDano;
    }
}