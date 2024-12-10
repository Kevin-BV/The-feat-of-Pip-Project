using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaquePersonaje : MonoBehaviour
{
    [Header("Configuración del ataque")]
    public float rangoDeAtaque = 1f; // Radio del ataque
    public int daño = 1; // Daño que inflige el ataque
    public Transform puntoDeAtaque; // Punto desde donde se detectará el ataque
    public LayerMask capaEnemigos; // Capa de los enemigos para detectar colisiones

    [Header("Cooldown del ataque")]
    public float tiempoEntreAtaques = 0.5f; // Tiempo de cooldown entre ataques

    [Header("Audio")]
    public AudioClip ataquefosforo;

    private Animator anim;
    private float tiempoDelUltimoAtaque = 0f; // Momento en que se hizo el último ataque
    private AudioSource audioSource;

    // Referencia al collider del punto de ataque (si lo usas)
    private Collider colliderDeAtaque;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        colliderDeAtaque = puntoDeAtaque.GetComponent<Collider>();

        // Desactiva el collider de ataque al inicio
        if (colliderDeAtaque != null)
        {
            colliderDeAtaque.enabled = false;
        }
    }

    void Update()
    {
        // Si se presiona el botón izquierdo del ratón, ha pasado el cooldown y no está recibiendo daño
        if (Input.GetMouseButtonDown(0) && Time.time >= tiempoDelUltimoAtaque + tiempoEntreAtaques && !anim.GetBool("IsDamaged"))
        {
            EjecutarAtaque();
        }
    }

    private void EjecutarAtaque()
    {
        // Reproducir la animación de ataque
        anim.SetTrigger("Atacar");

        // Reproducir el sonido del ataque
        if (ataquefosforo != null && audioSource != null)
        {
            audioSource.PlayOneShot(ataquefosforo);
        }

        // Registrar el tiempo del ataque
        tiempoDelUltimoAtaque = Time.time;

        // Realizar daño directo con Physics.OverlapSphere
        HacerDaño();

        // Activar temporalmente el collider de ataque (si se usa)
        if (colliderDeAtaque != null)
        {
            colliderDeAtaque.enabled = true;
            StartCoroutine(DesactivarColliderDespuesDeTiempo(0.1f)); // Ajusta el tiempo según la duración de la animación
        }
    }

    private void HacerDaño()
    {
        // Detectar enemigos dentro del rango usando Physics.OverlapSphere
        Collider[] enemigosEnRango = Physics.OverlapSphere(puntoDeAtaque.position, rangoDeAtaque, capaEnemigos);

        foreach (Collider enemigo in enemigosEnRango)
        {
            // Llamar a la función RecibirDano de los enemigos si la tienen
            if (enemigo.TryGetComponent(out Rata rata))
            {
                rata.RecibirDano(daño);
            }
        }
    }

    private IEnumerator DesactivarColliderDespuesDeTiempo(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        if (colliderDeAtaque != null)
        {
            colliderDeAtaque.enabled = false;
        }
    }

    // Mostrar el rango de ataque en el editor
    private void OnDrawGizmosSelected()
    {
        if (puntoDeAtaque != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(puntoDeAtaque.position, rangoDeAtaque);
        }
    }
}