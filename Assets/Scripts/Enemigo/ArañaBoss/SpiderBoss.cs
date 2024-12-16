using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpiderBoss : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform player;

    public int vida = 50;
    private Animator animator;

    public Collider ataqueCollider;
    private int attackCounter = 0;

    private bool jugadorDentroCollider = false;
    private float tiempoUltimoAtaque = 0f;
    public float intervaloAtaque = 4f;

    public Image barraVida;

    private bool estaMuerta = false;
    public string escenaCreditos = "Creditos";

    public AudioSource audioSource;
    public AudioClip sonidoAraña;
    public AudioClip sonidoHurt;

    private bool estaReproduciendoSonidoAraña = false;

    private bool estaEnDamage = false; // Nueva variable para controlar el estado de daño

    // Nuevas variables para generar arañas pequeñas
    public GameObject arañaPequeñaPrefab; // Prefab de las arañas pequeñas
    public Transform puntoGeneracionArañas; // Punto de generación de las arañas pequeñas
    public float tiempoGeneracion50 = 5f; // Tiempo para generar arañas cuando tiene menos del 50% de vida
    public float tiempoGeneracion25 = 2f; // Tiempo para generar arañas cuando tiene menos del 25% de vida
    private float tiempoUltimaGeneracion = 0f; // Para controlar el tiempo entre generaciones
    private bool generandoArañas = false; // Para controlar si se están generando las arañas

    void Start()
    {
        animator = GetComponent<Animator>();
        ataqueCollider.isTrigger = true;
        ActualizarBarraVida();

        // Asegurar que el AudioSource se sincronice con el deslizador desde el inicio
        var volumenController = FindObjectOfType<VolumenSlider_Icono>();
        if (volumenController != null)
        {
            audioSource = volumenController.aranaSfxAudioSource;
            if (audioSource != null)
            {
                audioSource.volume = volumenController.sfxSlider.value;
            }
        }
    }

    void Update()
    {
        if (estaMuerta || estaEnDamage) return; // Bloquear movimiento y ataques si está muerta o en daño

        // Reproduce el sonido de la araña en bucle si no está ya reproduciéndose
        if (!estaReproduciendoSonidoAraña && sonidoAraña != null)
        {
            audioSource.clip = sonidoAraña;
            audioSource.loop = true;
            audioSource.Play();
            estaReproduciendoSonidoAraña = true;
        }

        // Lógica de movimiento y ataque
        if (player != null)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            transform.localScale = new Vector3(
                directionToPlayer.x < 0 ? Mathf.Abs(transform.localScale.x) : -Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }

        // Generación de arañas
        if (vida <= 25 && !generandoArañas) // Empieza a generar arañas al 25%
        {
            generandoArañas = true;
            tiempoGeneracion50 = tiempoGeneracion25; // Aumenta la velocidad de generación
        }

        if (vida <= 50 || vida <= 25) // Generar arañas a partir de 50% y 25% de vida
        {
            if (Time.time - tiempoUltimaGeneracion >= tiempoGeneracion50)
            {
                GenerarAraña();
                tiempoUltimaGeneracion = Time.time;
            }
        }

        // Lógica de ataque
        if (jugadorDentroCollider && Time.time - tiempoUltimoAtaque >= intervaloAtaque)
        {
            animator.SetTrigger(attackCounter < 3 ? "Attack_1" : "Attack_2");
            player.GetComponent<VidaConCorazones>().RecibirDano(attackCounter < 3 ? 1 : 2);
            attackCounter = (attackCounter + 1) % 4;
            tiempoUltimoAtaque = Time.time;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (estaMuerta) return;

        if (other.CompareTag("Player")) jugadorDentroCollider = true;
        else if (other.CompareTag("PlayerAttack"))
        {
            var jugador = other.GetComponentInParent<AtaquePersonaje>(); // Asegúrate de usar el script correcto
            if (jugador != null)
            {
                RecibirDano(jugador.dano); // Usa el daño dinámico del jugador
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (estaMuerta) return;

        if (other.CompareTag("Player")) jugadorDentroCollider = false;
    }

    public void RecibirDano(int dano)
    {
        if (estaMuerta || estaEnDamage) return; // Evitar recibir daño si está muerto o ya está en la animación de daño

        vida -= dano;
        ActualizarBarraVida();

        // Activar la animación de daño
        if (animator != null)
        {
            animator.SetTrigger("Damage");
            estaEnDamage = true; // Bloquear acciones mientras está en daño
        }

        if (sonidoHurt != null) audioSource.PlayOneShot(sonidoHurt);
        if (vida <= 0) Morir();
    }

    // Método para terminar la animación de daño
    public void TerminarAnimacionDanio()
    {
        estaEnDamage = false; // Permitir acciones nuevamente
    }

    private void Morir()
    {
        if (estaMuerta) return;

        estaMuerta = true;
        animator.SetTrigger("Death");
        audioSource.Stop();
        ataqueCollider.enabled = false;

        Invoke("CambiarEscenaCreditos", 5f);
    }

    private void CambiarEscenaCreditos()
    {
        if (!string.IsNullOrEmpty(escenaCreditos))
            SceneManager.LoadScene(escenaCreditos);
    }

    private void ActualizarBarraVida()
    {
        barraVida.fillAmount = (float)vida / 50f;
    }

    private void GenerarAraña()
    {
        if (arañaPequeñaPrefab != null && puntoGeneracionArañas != null)
        {
            Instantiate(arañaPequeñaPrefab, puntoGeneracionArañas.position, Quaternion.identity);
        }
    }
}