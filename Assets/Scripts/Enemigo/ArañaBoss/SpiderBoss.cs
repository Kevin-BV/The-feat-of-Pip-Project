using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpiderBoss : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float moveAmount = 7f;
    public float waitTime = 10f;
    public Transform player;
    public float gizmoSize = 1f;

    public int vida = 50;
    private Animator animator;

    public Collider ataqueCollider;

    private int attackCounter = 0;

    private float tiempoUltimoDanio = 0f;
    public float intervaloDanio = 1f;

    private bool jugadorDentroCollider = false;
    private float tiempoUltimoAtaque = 0f;
    public float intervaloAtaque = 4f;

    public Image barraVida;
    public Image bordeBarraVida;

    private bool estaMuerta = false;

    [Tooltip("Nombre de la escena de créditos")]
    public string escenaCreditos = "Creditos";

    public AudioSource audioSource;
    public AudioClip sonidoAraña;
    public AudioClip sonidoHurt;

    private bool estaReproduciendoSonidoAraña = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        ataqueCollider.isTrigger = true;
        ActualizarBarraVida();

        audioSource = FindObjectOfType<VolumenSlider_Icono>().aranaSfxAudioSource;
    }

    void Update()
    {
        if (estaMuerta) return;

        // Reproduce el sonido de "sonidoAraña" constantemente
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

            if (directionToPlayer.x < 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }

        if (jugadorDentroCollider && Time.time - tiempoUltimoAtaque >= intervaloAtaque)
        {
            if (attackCounter < 3)
            {
                animator.SetTrigger("Attack_1");
                attackCounter++;
                player.GetComponent<VidaConCorazones>().RecibirDano(1);
            }
            else
            {
                animator.SetTrigger("Attack_2");
                attackCounter = 0;
                player.GetComponent<VidaConCorazones>().RecibirDano(2);
            }

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
            RecibirDano(1);
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
        if (estaMuerta) return;

        vida -= dano;
        ActualizarBarraVida();

        // Reproduce el sonido de daño
        if (sonidoHurt != null)
        {
            audioSource.PlayOneShot(sonidoHurt);
        }

        if (vida <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        if (estaMuerta) return;

        estaMuerta = true;
        animator.SetTrigger("Death");

        // Detiene el sonido constante y cualquier otra reproducción
        audioSource.Stop();

        ataqueCollider.enabled = false;

        Invoke("CambiarEscenaCreditos", 5f);
    }

    private void CambiarEscenaCreditos()
    {
        if (!string.IsNullOrEmpty(escenaCreditos))
        {
            SceneManager.LoadScene(escenaCreditos);
        }
    }

    private void ActualizarBarraVida()
    {
        float porcentajeVida = (float)vida / 50f;
        barraVida.fillAmount = porcentajeVida;
    }
}
