using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaquePersonaje : MonoBehaviour
{
    public float rangoDeAtaque = 1f; // Radio del ataque
    public int da�o = 10; // Da�o que inflige el ataque
    public Transform puntoDeAtaque; // Punto desde donde se detectar� el ataque
    public LayerMask capaEnemigos; // Capa de los enemigos para detectar colisiones

    private Animator anim;
    private bool enAtaque = false; // Variable para bloquear ataques mientras uno est� activo

    void Start()
    {
        // Obtenemos el Animator para controlar las animaciones
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Detectamos si se presiona la tecla Z y si no estamos en ataque
        if (Input.GetKeyDown(KeyCode.Z) && !enAtaque) // Cambi� de "Fire1" a "Z"
        {
            RealizarAtaque();
        }
    }

    void RealizarAtaque()
    {
        // Activamos la animaci�n de ataque (se activa el trigger)
        anim.SetTrigger("Atacar");
        enAtaque = true; // Bloqueamos el ataque hasta que la animaci�n termine

        // Detectamos los enemigos dentro del rango de ataque
        Collider[] enemigos = Physics.OverlapSphere(puntoDeAtaque.position, rangoDeAtaque, capaEnemigos);

        foreach (Collider enemigo in enemigos)
        {
            // Aplicamos da�o al enemigo detectado si tiene un componente "Enemigo"
            enemigo.GetComponent<Enemigo>()?.TomarDa�o(da�o);
        }
    }

    // M�todo llamado desde el Animator al final de la animaci�n de ataque
    public void FinalizarAtaque()
    {
        enAtaque = false; // Permitimos que el personaje pueda atacar de nuevo
    }

    // Visualizaci�n del rango de ataque en la escena
    private void OnDrawGizmosSelected()
    {
        if (puntoDeAtaque != null)
        {
            Gizmos.color = Color.red; // Color rojo para el gizmo de ataque
            Gizmos.DrawWireSphere(puntoDeAtaque.position, rangoDeAtaque); // Dibujamos el rango de ataque
        }
    }
}