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

    private Collider colliderDeAtaque; // Referencia al collider del punto de ataque

    void Start()
    {
        anim = GetComponent<Animator>();
        colliderDeAtaque = puntoDeAtaque.GetComponent<Collider>();

        // Asegúrate de que el collider esté desactivado al inicio
        if (colliderDeAtaque != null)
        {
            colliderDeAtaque.enabled = false;
        }
    }

    void Update()
    {
        // Click izquierdo: activa el collider para el ataque
        if (Input.GetMouseButtonDown(0) && Time.time >= tiempoDelUltimoAtaque + tiempoEntreAtaques)
        {
            ActivarColliderDeAtaque();
        }

        // Click izquierdo: daño directo con Physics.OverlapSphere
        if (Input.GetMouseButtonDown(0) && Time.time >= tiempoDelUltimoAtaque + tiempoEntreAtaques)
        {
            HacerDañoDirecto();
        }
    }

    private void ActivarColliderDeAtaque()
    {
        anim.SetTrigger("Atacar");
        tiempoDelUltimoAtaque = Time.time;

        // Activar el collider temporalmente
        if (colliderDeAtaque != null)
        {
            colliderDeAtaque.enabled = true;
            StartCoroutine(DesactivarColliderDespuesDeTiempo(0.1f)); // Ajusta el tiempo según la animación
        }
    }

    private void HacerDañoDirecto()
    {
        anim.SetTrigger("Atacar");
        tiempoDelUltimoAtaque = Time.time;

        // Detectar enemigos dentro del rango usando Physics.OverlapSphere
        Collider[] enemigosEnRango = Physics.OverlapSphere(puntoDeAtaque.position, rangoDeAtaque, capaEnemigos);

        foreach (Collider enemigo in enemigosEnRango)
        {
            // Llamar a la función RecibirDano de los enemigos si la tienen
            if (enemigo.TryGetComponent(out Rata rata))
            {
                rata.RecibirDano(daño);
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

    // Mostrar el rango de ataque en el editor
    private void OnDrawGizmosSelected()
    {
        if (puntoDeAtaque != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(puntoDeAtaque.position, rangoDeAtaque);
        }
    }
}