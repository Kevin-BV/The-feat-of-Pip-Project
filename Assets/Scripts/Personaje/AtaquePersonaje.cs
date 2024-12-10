using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaquePersonaje : MonoBehaviour
{
    [Header("Configuraci�n del ataque")]
    public float rangoDeAtaque = 1f; // Radio del ataque
    public int da�o = 1; // Da�o que inflige el ataque
    public Transform puntoDeAtaque; // Punto desde donde se detectar� el ataque
    public LayerMask capaEnemigos; // Capa de los enemigos para detectar colisiones

    [Header("Cooldown del ataque")]
    public float tiempoEntreAtaques = 0.5f; // Tiempo de cooldown entre ataques

    [Header("Audio")]
    public AudioClip ataquefosforo;

    private Animator anim;
    private float tiempoDelUltimoAtaque = 0f; // Momento en que se hizo el �ltimo ataque
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
        // Si se presiona el bot�n izquierdo del rat�n, ha pasado el cooldown y no est� recibiendo da�o
        if (Input.GetMouseButtonDown(0) && Time.time >= tiempoDelUltimoAtaque + tiempoEntreAtaques && !anim.GetBool("IsDamaged"))
        {
            EjecutarAtaque();
        }
    }

    private void EjecutarAtaque()
    {
        // Reproducir la animaci�n de ataque
        anim.SetTrigger("Atacar");

        // Reproducir el sonido del ataque
        if (ataquefosforo != null && audioSource != null)
        {
            audioSource.PlayOneShot(ataquefosforo);
        }

        // Registrar el tiempo del ataque
        tiempoDelUltimoAtaque = Time.time;

        // Realizar da�o directo con Physics.OverlapSphere
        HacerDa�o();

        // Activar temporalmente el collider de ataque (si se usa)
        if (colliderDeAtaque != null)
        {
            colliderDeAtaque.enabled = true;
            StartCoroutine(DesactivarColliderDespuesDeTiempo(0.1f)); // Ajusta el tiempo seg�n la duraci�n de la animaci�n
        }
    }

    private void HacerDa�o()
    {
        // Detectar enemigos dentro del rango usando Physics.OverlapSphere
        Collider[] enemigosEnRango = Physics.OverlapSphere(puntoDeAtaque.position, rangoDeAtaque, capaEnemigos);

        foreach (Collider enemigo in enemigosEnRango)
        {
            // Llamar a la funci�n RecibirDano de los enemigos si la tienen
            if (enemigo.TryGetComponent(out Rata rata))
            {
                rata.RecibirDano(da�o);
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