using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaquePersonaje : MonoBehaviour
{
    public float rangoDeAtaque = 1f; // Radio del ataque
    public int daño = 10; // Daño que inflige el ataque
    public Transform puntoDeAtaque; // Punto desde donde se detectará el ataque
    public LayerMask capaEnemigos; // Capa de los enemigos para detectar colisiones

    private Animator anim;
    private float tiempoEntreAtaques = 0.5f; // Tiempo de cooldown entre ataques
    private float tiempoDelUltimoAtaque = 0f; // Momento en que se hizo el último ataque

    void Start()
    {
        // Obtenemos el Animator para controlar las animaciones
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Detectamos si se presiona la tecla Z y si ha pasado suficiente tiempo para realizar otro ataque
        if (Input.GetKeyDown(KeyCode.Z) && Time.time >= tiempoDelUltimoAtaque + tiempoEntreAtaques)
        {
            RealizarAtaque();
        }
    }

    void RealizarAtaque()
    {
        // Activamos la animación de ataque (se activa el trigger)
        anim.SetTrigger("Atacar");

        // Registramos el tiempo en que se realiza este ataque
        tiempoDelUltimoAtaque = Time.time;

        // Detectamos los enemigos dentro del rango de ataque
        Collider[] enemigos = Physics.OverlapSphere(puntoDeAtaque.position, rangoDeAtaque, capaEnemigos);

        foreach (Collider enemigo in enemigos)
        {
            // Aplicamos daño al enemigo detectado si tiene un componente "Enemigo"
            enemigo.GetComponent<Enemigo>()?.TomarDaño(daño);
        }
    }

    // Visualización del rango de ataque en la escena
    private void OnDrawGizmosSelected()
    {
        if (puntoDeAtaque != null)
        {
            Gizmos.color = Color.red; // Color rojo para el gizmo de ataque
            Gizmos.DrawWireSphere(puntoDeAtaque.position, rangoDeAtaque); // Dibujamos el rango de ataque
        }
    }
}