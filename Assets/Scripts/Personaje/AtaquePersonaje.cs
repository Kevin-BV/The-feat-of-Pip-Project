using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaquePersonaje : MonoBehaviour
{
    [Header("Configuración del ataque")]
    public float rangoDeAtaque = 1f; // Radio del ataque
    public int dano = 1; // Daño que inflige el ataque
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
        if (Input.GetMouseButtonDown(0) && Time.time >= tiempoDelUltimoAtaque + tiempoEntreAtaques && !anim.GetBool("IsDamaged"))
        {
            EjecutarAtaque();
        }
    }

    private void EjecutarAtaque()
    {
        anim.SetTrigger("Atacar");

        if (ataquefosforo != null && audioSource != null)
        {
            audioSource.PlayOneShot(ataquefosforo);
        }

        tiempoDelUltimoAtaque = Time.time;

        HacerDaño();

        if (colliderDeAtaque != null)
        {
            colliderDeAtaque.enabled = true;
            StartCoroutine(DesactivarColliderDespuesDeTiempo(0.1f));
        }
    }

    private void HacerDaño()
    {
        Collider[] enemigosEnRango = Physics.OverlapSphere(puntoDeAtaque.position, rangoDeAtaque, capaEnemigos);

        foreach (Collider enemigo in enemigosEnRango)
        {
            if (enemigo.TryGetComponent(out Rata rata))
            {
                rata.RecibirDano(dano);
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

    // Método público para modificar el daño desde otros scripts
    public void ModificarDano(int nuevoDano)
    {
        Debug.Log($"Modificando daño: {dano} -> {nuevoDano}");
        dano = nuevoDano;
    }

    private void OnDrawGizmosSelected()
    {
        if (puntoDeAtaque != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(puntoDeAtaque.position, rangoDeAtaque);
        }
    }
}