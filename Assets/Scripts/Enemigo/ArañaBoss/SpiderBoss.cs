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
    public string escenaCreditos = "Creditos"; // Nombre de la escena, configurable desde el inspector
    public float tiempoCambioEscena = 5f; // Tiempo de espera antes de cambiar de escena

    public AudioSource audioSource;
    public AudioClip sonidoAraña;
    public AudioClip sonidoHurt;

    private bool estaReproduciendoSonidoAraña = false;

    private bool estaEnDamage = false; // Nueva variable para controlar el estado de daño

    public GameObject arañaPequeñaPrefab;
    public Transform puntoGeneracionArañas;
    public float tiempoGeneracion50 = 15f;
    public float tiempoGeneracion25 = 10f;
    private float tiempoUltimaGeneracion = 0f;
    private bool generandoArañas = false;

    // Nueva referencia para el objeto del HUD y para congelar la escena
    public GameObject hudObjetoMuerte;  // El objeto que se activa cuando la araña muere
    private bool escenaCongelada = false; // Para evitar congelar más de una vez la escena

    void Start()
    {
        animator = GetComponent<Animator>();
        ataqueCollider.isTrigger = true;
        ActualizarBarraVida();
    }

    void Update()
    {
        if (estaMuerta || estaEnDamage) return;

        // Movimiento hacia el jugador
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
        if (vida <= 25 && !generandoArañas)
        {
            generandoArañas = true;
            tiempoGeneracion50 = tiempoGeneracion25; // Cambia la velocidad de generación
        }

        if (vida <= 25 || vida <= 10)
        {
            if (Time.time - tiempoUltimaGeneracion >= tiempoGeneracion50)
            {
                GenerarAraña();
                tiempoUltimaGeneracion = Time.time;
            }
        }

        // Ataque
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

        if (other.CompareTag("Player"))
        {
            jugadorDentroCollider = true;
        }
        else if (other.CompareTag("PlayerAttack"))
        {
            var jugador = other.GetComponentInParent<AtaquePersonaje>();
            if (jugador != null)
            {
                RecibirDano(jugador.dano);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (estaMuerta) return;

        if (other.CompareTag("Player"))
        {
            jugadorDentroCollider = false;
        }
    }

    public void RecibirDano(int dano)
    {
        if (estaMuerta || estaEnDamage) return;

        vida -= dano;
        Debug.Log("Vida de la araña: " + vida);  // Esto mostrará el valor de vida en la consola
        ActualizarBarraVida();

        if (animator != null)
        {
            animator.SetTrigger("Damage");
            estaEnDamage = true;
        }

        if (sonidoHurt != null)
        {
            audioSource.PlayOneShot(sonidoHurt);
        }

        if (vida <= 0)
        {
            Morir();
        }
    }

    public void TerminarAnimacionDanio()
    {
        estaEnDamage = false;
    }

    private void Morir()
    {
        if (estaMuerta) return;

        Debug.Log("La araña ha muerto");  // Esto debería mostrarse en la consola

        estaMuerta = true;
        animator.SetTrigger("Death");
        audioSource.Stop();
        ataqueCollider.enabled = false;


        // Iniciar Coroutine para destruir el objeto después de 2 segundos
        StartCoroutine(DestruirDespuesDeTiempo(4f));
    }

    private IEnumerator DestruirDespuesDeTiempo(float tiempoEspera)
    {
        yield return new WaitForSecondsRealtime(tiempoEspera); // Usamos WaitForSecondsRealtime para evitar el efecto de Time.timeScale

        Debug.Log("Destruyendo el objeto SpiderBoss");  // Esto debería mostrarse en la consola
        Destroy(gameObject);
    }

    private void CambiarEscenaCreditos()
    {
        if (!string.IsNullOrEmpty(escenaCreditos))
        {
            // Intenta cargar la escena configurada
            if (SceneUtility.GetBuildIndexByScenePath(escenaCreditos) != -1)
            {
                SceneManager.LoadScene(escenaCreditos);
            }
            else
            {
                Debug.LogError($"La escena '{escenaCreditos}' no está incluida en la lista de escenas del build.");
            }
        }
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