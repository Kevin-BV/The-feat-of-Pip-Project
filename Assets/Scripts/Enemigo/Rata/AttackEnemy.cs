using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : MonoBehaviour
{
    public Transform attackEnemy; // El Empty que marcará el punto de ataque
    public float attackRange = 1f; // Rango de ataque (distancia desde el punto de ataque)
    public float attackCooldown = 3f; // Tiempo entre ataques
    private float attackTimer = 0f; // Temporizador del ataque

    public Animator animator; // Referencia al Animator del enemigo
    public string playerTag = "Player"; // El tag del jugador

    void Update()
    {
        // Aumentar el temporizador
        attackTimer += Time.deltaTime;

        // Verificar si el temporizador ha llegado al cooldown
        if (attackTimer >= attackCooldown)
        {
            // Verificar si hay un jugador dentro del rango de ataque
            Collider[] playerInRange = Physics.OverlapSphere(attackEnemy.position, attackRange);

            foreach (Collider player in playerInRange)
            {
                // Comprobar si el objeto tiene el tag "Player"
                if (player.CompareTag(playerTag))
                {
                    // Si está dentro del rango y es el jugador, realizar el ataque
                    StartCoroutine(PerformAttack(player)); // Iniciar la corutina para aplicar el daño con retraso
                    break; // Detener la búsqueda si ya se ha encontrado al jugador
                }
            }

            // Resetear el temporizador
            attackTimer = 0f;
        }
    }

    // Coroutine para aplicar el daño con un retraso de 0.5 segundos
    IEnumerator PerformAttack(Collider player)
    {
        // Activar la animación de ataque
        animator.SetTrigger("Attack");

        // Esperar 0.5 segundos antes de aplicar el daño
        yield return new WaitForSeconds(0.5f);

        // Infligir daño al jugador
        VidaConCorazones vidaJugador = player.GetComponent<VidaConCorazones>();
        if (vidaJugador != null)
        {
            vidaJugador.RecibirDano(1); // Infligir 1 de daño
        }
    }

    // Mostrar el rango de ataque en la vista de la escena para facilitar la configuración
    private void OnDrawGizmosSelected()
    {
        if (attackEnemy != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackEnemy.position, attackRange);
        }
    }
}