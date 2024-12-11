using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaquePersonaje : MonoBehaviour
{
    [Header("Configuraci�n del ataque")]
    public float rangoDeAtaque = 1f; // Radio del ataque
    public int dano = 1; // Da�o que inflige el ataque
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

        HacerDa�o();

        if (colliderDeAtaque != null)
        {
            colliderDeAtaque.enabled = true;
            StartCoroutine(DesactivarColliderDespuesDeTiempo(0.1f));
        }
    }

    private void HacerDa�o()
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

    // M�todo p�blico para modificar el da�o desde otros scripts
    public void ModificarDano(int nuevoDano)
    {
        Debug.Log($"Modificando da�o: {dano} -> {nuevoDano}");
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