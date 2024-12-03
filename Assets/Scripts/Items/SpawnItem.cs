using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    [Header("Configuración de Spawn")]
    public GameObject objetoASpawnear;  // Prefab que se va a spawnear
    public float tiempoDeEspera = 3f;   // Tiempo en segundos entre cada spawn
    public Vector3 rangoDeSpawn = new Vector3(5f, 0f, 5f); // Rango para spawn en el espacio 3D

    [Header("Configuración de Sonidos (opcional)")]
    public AudioSource audioSource;
    public AudioClip sonidoSpawn;

    private void Start()
    {
        // Iniciar el spawn continuo en segundo plano
        StartCoroutine(SpawnContinuo());
    }

    private IEnumerator SpawnContinuo()
    {
        // Continuamente spawn objetos a intervalos establecidos
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(tiempoDeEspera);  // Espera el tiempo de spawn antes de crear otro objeto
        }
    }

    private void Spawn()
    {
        if (objetoASpawnear != null)
        {
            // Generar una posición aleatoria dentro del rango
            Vector3 posicionAleatoria = new Vector3(
                transform.position.x + Random.Range(-rangoDeSpawn.x, rangoDeSpawn.x),
                transform.position.y,
                transform.position.z + Random.Range(-rangoDeSpawn.z, rangoDeSpawn.z)
            );

            // Instanciar el objeto en la posición aleatoria
            Instantiate(objetoASpawnear, posicionAleatoria, Quaternion.identity);

            // Reproducir sonido de spawn si se ha asignado un AudioSource y Clip
            if (audioSource != null && sonidoSpawn != null)
            {
                audioSource.PlayOneShot(sonidoSpawn);
            }
        }
        else
        {
            Debug.LogWarning("No se ha asignado un prefab para spawnear.");
        }
    }
}