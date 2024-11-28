using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaquePersonaje : MonoBehaviour
{
    public float rangoDeAtaque = 1f; // Radio del ataque
    public int daño = 1; // Daño que inflige el ataque
    public Transform puntoDeAtaque; // Punto desde donde se detectará el ataque
    public LayerMask capaEnemigos; // Capa de los enemigos para detectar colisiones

    private Animator anim;
    private float tiempoEntreAtaques = 0.5f; // Tiempo de cooldown entre ataques
    private float tiempoDelUltimoAtaque = 0f; // Momento en que se hizo el último ataque
    private bool atacando = false; // Variable para controlar si está realizando el ataque

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && Time.time >= tiempoDelUltimoAtaque + tiempoEntreAtaques)
        {
            RealizarAtaque();
        }
    }

    void RealizarAtaque()
    {
        anim.SetTrigger("Atacar");
        tiempoDelUltimoAtaque = Time.time;
        atacando = true;

        // Activamos el trigger o área de daño del personaje
        puntoDeAtaque.GetComponent<Collider>().enabled = true;

        // Detectamos enemigos en el rango
        Collider[] enemigos = Physics.OverlapSphere(puntoDeAtaque.position, rangoDeAtaque, capaEnemigos);

        foreach (Collider enemigo in enemigos)
        {
            Enemigo enemigoScript = enemigo.GetComponent<Enemigo>();
            if (enemigoScript != null && Vector3.Distance(puntoDeAtaque.position, enemigo.transform.position) <= rangoDeAtaque)
            {
                enemigoScript.TomarDaño(daño);
                Debug.Log("El enemigo está recibiendo daño");
            }
        }
    }

    private void ResetearAtaque()
    {
        atacando = false;

        // Desactivamos el trigger del área de ataque para la siguiente vez
        puntoDeAtaque.GetComponent<Collider>().enabled = false;
    }

    // Mostrar el rango de ataque solo cuando el personaje esté atacando
    private void OnDrawGizmosSelected()
    {
        if (puntoDeAtaque != null && atacando)
        {
            Gizmos.color = Color.red; // Color rojo para el gizmo
            Gizmos.DrawWireSphere(puntoDeAtaque.position, rangoDeAtaque); // Dibujamos el rango de ataque
        }
    }

    // Reseteamos la variable "atacando" después de un tiempo
}