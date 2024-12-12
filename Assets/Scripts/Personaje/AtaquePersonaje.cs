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

    [Header("Power-Up")]
    public GameObject efectoPowerUp; // GameObject del ParticleSystem hijo del jugador
    public float duracionPowerUp = 10f; // Duraci�n del efecto Power-Up

    private Animator anim;
    private float tiempoDelUltimoAtaque = 0f; // Momento en que se hizo el �ltimo ataque
    private AudioSource audioSource;

    private bool jugadorDentroDePowerUp = false; // Verifica si el jugador est� dentro del trigger del Power-Up
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

        // Desactiva el efecto visual al inicio
        if (efectoPowerUp != null)
        {
            efectoPowerUp.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= tiempoDelUltimoAtaque + tiempoEntreAtaques && !anim.GetBool("IsDamaged"))
        {
            EjecutarAtaque();
        }

        // Activa el Power-Up al presionar X si est� dentro del trigger
        if (jugadorDentroDePowerUp && Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine(ActivarPowerUp());
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

    public void ModificarDano(int nuevoDano)
    {
        Debug.Log($"Modificando da�o: {dano} -> {nuevoDano}");
        dano = nuevoDano;
    }

    private IEnumerator ActivarPowerUp()
    {
        // Aseg�rate de que el Power-Up no se active m�ltiples veces
        jugadorDentroDePowerUp = false;

        // Almacena el da�o original
        int danoOriginal = dano;

        // Duplica el da�o y activa el efecto visual
        ModificarDano(danoOriginal * 2);
        Debug.Log("Power-Up activado. Da�o duplicado a: " + dano);
        if (efectoPowerUp != null)
        {
            efectoPowerUp.SetActive(true);
        }

        // Espera la duraci�n del Power-Up
        yield return new WaitForSeconds(duracionPowerUp);

        // Restaura el da�o original y desactiva el efecto visual
        ModificarDano(danoOriginal);
        Debug.Log("Power-Up finalizado. Da�o restaurado a: " + dano);
        if (efectoPowerUp != null)
        {
            efectoPowerUp.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si entra en el trigger de un objeto con el tag PowerUp
        if (other.CompareTag("PowerUp"))
        {
            jugadorDentroDePowerUp = true;
            Debug.Log("Jugador dentro del trigger del Power-Up.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Si sale del trigger del objeto PowerUp
        if (other.CompareTag("PowerUp"))
        {
            jugadorDentroDePowerUp = false;
            Debug.Log("Jugador sali� del trigger del Power-Up.");
        }
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