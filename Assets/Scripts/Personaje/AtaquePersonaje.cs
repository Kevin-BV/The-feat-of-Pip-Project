using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Power-Up")]
    public GameObject efectoPowerUp; // GameObject del ParticleSystem hijo del jugador
    public float duracionPowerUp = 8f; // Duración del efecto Power-Up
    public GameObject objetoPowerUp; // Referencia al objeto de Power-Up en la escena

    [Header("UI del Power-Up")]
    public Image[] barraPowerUp; // Arreglo de imágenes para la barra del Power-Up

    private Animator anim;
    private float tiempoDelUltimoAtaque = 0f; // Momento en que se hizo el último ataque
    private AudioSource audioSource;

    private bool jugadorDentroDePowerUp = false; // Verifica si el jugador está dentro del trigger del Power-Up
    private Collider colliderDeAtaque;

    private int contadorUsosPowerUp = 0; // Contador de activaciones del Power-Up
    private const int maxUsosPowerUp = 2; // Máximo de usos permitidos

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

        // Asegúrate de que solo una imagen esté activa al inicio
        ResetBarraPowerUp();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= tiempoDelUltimoAtaque + tiempoEntreAtaques && !anim.GetBool("IsDamaged"))
        {
            EjecutarAtaque();
        }

        // Activa el Power-Up al presionar X si está dentro del trigger
        if (jugadorDentroDePowerUp && Input.GetKeyDown(KeyCode.X))
        {
            ActivarPowerUpConLimite();
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

    private void ActivarPowerUpConLimite()
    {
        if (contadorUsosPowerUp < maxUsosPowerUp)
        {
            contadorUsosPowerUp++;
            Debug.Log($"Power-Up activado. Usos restantes: {maxUsosPowerUp - contadorUsosPowerUp}");

            StartCoroutine(ActivarPowerUp());
        }
        else
        {
            // Destruye el objeto de Power-Up después de 2 usos
            if (objetoPowerUp != null)
            {
                Debug.Log("Power-Up agotado y destruido.");
                Destroy(objetoPowerUp);
            }
        }
    }

    private IEnumerator ActivarPowerUp()
    {
        // Asegúrate de que el Power-Up no se active múltiples veces
        jugadorDentroDePowerUp = false;

        // Almacena el daño original
        int danoOriginal = dano;

        // Duplica el daño y activa el efecto visual
        ModificarDano(danoOriginal * 2);
        Debug.Log("Power-Up activado. Daño duplicado a: " + dano);
        if (efectoPowerUp != null)
        {
            efectoPowerUp.SetActive(true);
        }

        // Inicia la cuenta regresiva del Power-Up
        for (int i = 0; i < barraPowerUp.Length; i++)
        {
            barraPowerUp[i].enabled = true;
            if (i > 0) barraPowerUp[i - 1].enabled = false;
            yield return new WaitForSeconds(duracionPowerUp / barraPowerUp.Length);
        }

        // Apaga todas las imágenes al finalizar
        ResetBarraPowerUp();

        // Restaura el daño original y desactiva el efecto visual
        ModificarDano(danoOriginal);
        Debug.Log("Power-Up finalizado. Daño restaurado a: " + dano);
        if (efectoPowerUp != null)
        {
            efectoPowerUp.SetActive(false);
        }
    }

    public void ModificarDano(int nuevoDano)
    {
        Debug.Log($"Modificando daño: {dano} -> {nuevoDano}");
        dano = nuevoDano;
    }

    private void ResetBarraPowerUp()
    {
        foreach (var imagen in barraPowerUp)
        {
            imagen.enabled = false;
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
            Debug.Log("Jugador salió del trigger del Power-Up.");
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