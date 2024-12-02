using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rata : MonoBehaviour
{
    public float velocidadMovimiento = 3f; // Velocidad de movimiento de la rata
    public float rangoDeDeteccion = 5f; // Rango del gizmo para detectar al jugador
    public float rangoDeAcercamiento = 1.5f; // Distancia mínima a la que se detiene del jugador
    public Transform visual; // Objeto visual para animaciones
    public Animator anim; // Animator para las animaciones ("idle", "walk", "attack", "death")

    [Header("Configuración del Gizmo")]
    public float tamanoGizmo = 1f; // Rango de ataque

    [Header("Sistema de Vida")]
    public int vidaMaxima = 4; // Vida máxima
    private int vidaActual; // Vida actual

    private Transform jugador; // Referencia al jugador
    private bool mirandoDerecha = true; // Control de dirección

    private bool estaSiguiendo = false; // Controla si está siguiendo al jugador
    private bool puedeAtacar = true; // Controla si puede atacar

    private Vector3 posicionAnterior; // Para verificar si hay movimiento

    void Start()
    {
        vidaActual = vidaMaxima;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            jugador = playerObj.transform;
        }

        posicionAnterior = transform.position; // Inicializar la posición previa
    }

    void Update()
    {
        if (vidaActual <= 0) return;

        if (jugador == null)
        {
            anim.SetBool("Walk", false);
            return;
        }

        // Detectar al jugador en el rango en X
        float distanciaX = Mathf.Abs(transform.position.x - jugador.position.x); // Solo mide distancia en X
        estaSiguiendo = distanciaX <= rangoDeDeteccion;

        if (estaSiguiendo)
        {
            SeguirJugador();
        }

        // Atacar si está en rango
        float distanciaXZ = Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z),
                                             new Vector3(jugador.position.x, 0f, jugador.position.z));
        if (distanciaXZ <= tamanoGizmo && puedeAtacar)
        {
            AtacarJugador();
        }

        // Verificar si realmente está en movimiento
        VerificarMovimiento();
    }

    private void SeguirJugador()
    {
        Vector3 direccion = new Vector3(jugador.position.x - transform.position.x, 0f, jugador.position.z - transform.position.z).normalized;

        float distanciaXZ = Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z),
                                             new Vector3(jugador.position.x, 0f, jugador.position.z));

        if (distanciaXZ > rangoDeAcercamiento)
        {
            // Movimiento solo en X y Z
            transform.position += new Vector3(direccion.x, 0f, direccion.z) * velocidadMovimiento * Time.deltaTime;

            // Girar si cambia de dirección
            if ((direccion.x > 0 && !mirandoDerecha) || (direccion.x < 0 && mirandoDerecha))
            {
                Girar();
            }
        }
    }

    private void VerificarMovimiento()
    {
        // Si la posición actual es diferente de la anterior, está caminando
        if (transform.position != posicionAnterior)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }

        // Actualizar la posición anterior
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

    private System.Collections.IEnumerator CooldownAtaque()
    {
        puedeAtacar = false;
        yield return new WaitForSeconds(1f); // Cooldown de ataque
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
        if (vidaActual <= 0) return;

        vidaActual -= cantidad;
        Debug.Log($"Rata recibió {cantidad} de daño. Vida restante: {vidaActual}");

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        anim.SetTrigger("Death");
        velocidadMovimiento = 0;
        Destroy(gameObject, 5f); // Destruir después de 5 segundos
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, 0f, transform.position.z), rangoDeDeteccion); // Rango de detección solo en X/Z

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, 0f, transform.position.z), tamanoGizmo); // Rango de ataque
    }

    // Detectar colisiones con un trigger
    private void OnTriggerEnter(Collider other)
    {
        // Comprobar si el objeto que colisiona tiene el tag de ataque del jugador
        if (other.CompareTag("PlayerAttack"))
        {
            RecibirDano(1); // Ajusta el daño según sea necesario
        }
    }
}