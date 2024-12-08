using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TutorialInputs : MonoBehaviour
{
    [Header("Objeto a activar/desactivar")]
    public GameObject targetObject; // El objeto que quieres activar/desactivar desde el inspector

    private void Start()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false); // Asegúrate de que el objeto está desactivado al inicio
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica si el objeto tiene el tag "Player"
        {
            if (targetObject != null)
            {
                targetObject.SetActive(true); // Activa el objeto
            }
            else
            {
                Debug.LogWarning("No se ha asignado ningún objeto en el inspector.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica si el objeto tiene el tag "Player"
        {
            if (targetObject != null)
            {
                targetObject.SetActive(false); // Desactiva el objeto
            }
        }
    }
}