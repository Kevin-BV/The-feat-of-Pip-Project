using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaquePersonaje : MonoBehaviour
{
    public float rangoDeAtaque = 1f; // Radio del ataque
    public int da�o = 1; // Da�o que inflige el ataque
    public Transform puntoDeAtaque; // Punto desde donde se detectar� el ataque
    public LayerMask capaEnemigos; // Capa de los enemigos para detectar colisiones

    private Animator anim;
    private float tiempoEntreAtaques = 0.5f; // Tiempo de cooldown entre ataques
    private float tiempoDelUltimoAtaque = 0f; // Momento en que se hizo el �ltimo ataque
    private bool atacando = false; // Variable para controlar si est� realizando el ataque

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

        // Activamos el trigger o �rea de da�o del personaje
        puntoDeAtaque.GetComponent<Collider>().enabled = true;

        // Detectamos enemigos en el rango
        Collider[] enemigos = Physics.OverlapSphere(puntoDeAtaque.position, rangoDeAtaque, capaEnemigos);

        foreach (Collider enemigo in enemigos)
        {
            Enemigo enemigoScript = enemigo.GetComponent<Enemigo>();
            if (enemigoScript != null && Vector3.Distance(puntoDeAtaque.position, enemigo.transform.position) <= rangoDeAtaque)
            {
                enemigoScript.TomarDa�o(da�o);
                Debug.Log("El enemigo est� recibiendo da�o");
            }
        }
    }

    private void ResetearAtaque()
    {
        atacando = false;

        // Desactivamos el trigger del �rea de ataque para la siguiente vez
        puntoDeAtaque.GetComponent<Collider>().enabled = false;
    }

    // Mostrar el rango de ataque solo cuando el personaje est� atacando
    private void OnDrawGizmosSelected()
    {
        if (puntoDeAtaque != null && atacando)
        {
            Gizmos.color = Color.red; // Color rojo para el gizmo
            Gizmos.DrawWireSphere(puntoDeAtaque.position, rangoDeAtaque); // Dibujamos el rango de ataque
        }
    }

    // Reseteamos la variable "atacando" despu�s de un tiempo
}