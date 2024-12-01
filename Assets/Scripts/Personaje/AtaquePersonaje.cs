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

        // Inicialmente desactivar el collider para evitar activaciones no deseadas.
        puntoDeAtaque.GetComponent<Collider>().enabled = false;
    }

    void Update()
    {
        // Solo realizar el ataque si se presiona la tecla Z y ha pasado el tiempo de cooldown
        if (Input.GetMouseButtonDown(0) && Time.time >= tiempoDelUltimoAtaque + tiempoEntreAtaques)
        {
            RealizarAtaque();
        }
    }

    void RealizarAtaque()
    {
        anim.SetTrigger("Atacar");
        tiempoDelUltimoAtaque = Time.time;
        atacando = true;

        // Activamos el collider para el �rea de da�o del personaje solo cuando el ataque se realiza
        puntoDeAtaque.GetComponent<Collider>().enabled = true;

        // Detectamos enemigos en el rango de ataque
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

        // Desactivar el collider despu�s de un tiempo para evitar colisiones continuas
        StartCoroutine(DesactivarColliderTemporariamente());
    }

    private IEnumerator DesactivarColliderTemporariamente()
    {
        // Esperar un tiempo y luego desactivar el collider
        yield return new WaitForSeconds(0.1f); // Ajusta el tiempo seg�n sea necesario
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
}