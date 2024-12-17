using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rata : MonoBehaviour
{
    public float velocidadMovimiento = 3f; // Velocidad de movimiento de la rata
    public float rangoDeDeteccion = 5f; // Rango del gizmo para detectar al jugador
    public float rangoDeAcercamiento = 1.5f; // Distancia m�nima a la que se detiene del jugador
    public Transform visual; // Objeto visual para animaciones
    public Animator anim; // Animator para las animaciones ("idle", "walk", "attack", "damage", "death")

    [Header("Configuraci�n del Gizmo")]
    public float tamanoGizmo = 1f; // Rango de ataque
    public Transform puntoDeAtaque; // Objeto vac�o para definir el rango de ataque (se puede modificar en el Inspector)

    [Header("Sistema de Vida")]
    public int vidaMaxima = 4; // Vida m�xima
    private int vidaActual; // Vida actual

    public Transform jugador; // Referencia al jugador
    private bool mirandoDerecha = true; // Control de direcci�n

    private bool estaSiguiendo = false; // Controla si est� siguiendo al jugador
    private bool puedeAtacar = true; // Controla si puede atacar
    private bool estaRecibiendoDano = false; // Controla si la rata est� en la animaci�n de da�o

    private Vector3 posicionAnterior; // Para verificar si hay movimiento

    [Header("Sistema de Sonido")]
    public AudioClip hiss; // Audio al caminar
    public AudioClip hisshurt; // Audio al recibir da�o
    public AudioSource audioSource;

    void Start()
    {
        vidaActual = vidaMaxima;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            jugador = playerObj.transform;
        }

        posicionAnterior = transform.position; // Inicializar la posici�n previa

        // Busca el AudioSource en el hijo "AudioSourceContainer"
        Transform audioContainer = transform.Find("AudioSourceContainer");
        if (audioContainer != null)
        {
            audioSource = audioContainer.GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogError("No se encontr� un AudioSourceContainer con un AudioSource en el GameObject de la Rata.");
        }
    }

    void Update()
    {
        if (vidaActual <= 0 || estaRecibiendoDano) return; // No hacer nada si est� muerta o recibiendo da�o

        if (jugador == null)
        {
            anim.SetBool("Walk", false);
            return;
        }

        float distanciaX = Mathf.Abs(transform.position.x - jugador.position.x); // Solo mide distancia en X
        estaSiguiendo = distanciaX <= rangoDeDeteccion;

        if (estaSiguiendo)
        {
            SeguirJugador();
        }

        float distanciaXZ = Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z),
                                             new Vector3(jugador.position.x, 0f, jugador.position.z));
        if (distanciaXZ <= tamanoGizmo && puedeAtacar)
        {
            AtacarJugador();
        }

        VerificarMovimiento();
    }

    private void SeguirJugador()
    {
        Vector3 direccion = new Vector3(jugador.position.x - transform.position.x, 0f, jugador.position.z - transform.position.z).normalized;

        float distanciaXZ = Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z),
                                             new Vector3(jugador.position.x, 0f, jugador.position.z));

        if (distanciaXZ > rangoDeAcercamiento)
        {
            transform.position += new Vector3(direccion.x, 0f, direccion.z) * velocidadMovimiento * Time.deltaTime;

            if ((direccion.x > 0 && !mirandoDerecha) || (direccion.x < 0 && mirandoDerecha))
            {
                Girar();
            }
        }
    }

    private void VerificarMovimiento()
    {
        if (transform.position != posicionAnterior)
        {
            anim.SetBool("Walk", true);

            if (!audioSource.isPlaying)
            {
                audioSource.clip = hiss;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            anim.SetBool("Walk", false);

            if (audioSource.clip == hiss && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        posicionAnterior = transform.position;
    }

    private void AtacarJugador()
    {
        anim.SetTrigger("Attack");
        VidaConCorazones vidaJugador = jugador.GetComponent<VidaConCorazones>();
        if (vidaJugador != null)
        {
            vidaJugador.RecibirDano(1);
        }

        StartCoroutine(CooldownAtaque());
    }

    private IEnumerator CooldownAtaque()
    {
        puedeAtacar = false;
        yield return new WaitForSeconds(1f);
        puedeAtacar = true;
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 nuevaEscala = visual.localScale;
        nuevaEscala.x *= -1;
        visual.localScale = nuevaEscala;
    }

    public void RecibirDano(int cantidad)
    {
        if (vidaActual <= 0 || estaRecibiendoDano) return;

        vidaActual -= cantidad;
        Debug.Log($"Rata recibi� {cantidad} de da�o. Vida restante: {vidaActual}");

        audioSource.PlayOneShot(hisshurt);
        anim.SetTrigger("Damage");

        StartCoroutine(DesactivarAtaqueDuranteDa�o());

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    private IEnumerator DesactivarAtaqueDuranteDa�o()
    {
        estaRecibiendoDano = true;
        puedeAtacar = false;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length); // Espera la duraci�n de la animaci�n "Damage"
        estaRecibiendoDano = false;
        puedeAtacar = true;
    }

    private void Morir()
    {
        anim.SetTrigger("Death");
        velocidadMovimiento = 0;

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        Destroy(gameObject, 5f);
    }

    // Modificado para usar el objeto Empty para el Gizmo
    // Modificado para usar el objeto Empty para el Gizmo
    private void OnDrawGizmosSelected()
    {
        if (puntoDeAtaque != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(puntoDeAtaque.position, tamanoGizmo); // Gizmo basado en el Empty y tama�o editable
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, 0f, transform.position.z), rangoDeDeteccion);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            RecibirDano(1);
        }
    }
}