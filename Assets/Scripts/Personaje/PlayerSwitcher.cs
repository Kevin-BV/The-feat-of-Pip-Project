using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSwitcher : MonoBehaviour
{
    [Header("Objetos para activar/desactivar")]
    public GameObject objectToActivate; // El objeto que se activará cuando se presione la tecla X
    public GameObject objectToDeactivate1; // El primer objeto que se desactivará
    public GameObject objectToDeactivate2; // El segundo objeto que se desactivará
    public GameObject objectToDeactivate3; // El tercer objeto que se desactivará

    private bool isInTriggerZone = false; // Para saber si el jugador está dentro del trigger

    private void Update()
    {
        // Si estamos dentro del trigger y presionamos la tecla X, activamos el objeto y desactivamos los demás
        if (isInTriggerZone && Input.GetKeyDown(KeyCode.X))
        {
            ActivateObject();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Cuando el jugador entra en la zona del trigger
        if (other.CompareTag("Player"))
        {
            isInTriggerZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Cuando el jugador sale de la zona del trigger
        if (other.CompareTag("Player"))
        {
            isInTriggerZone = false;
        }
    }

    private void ActivateObject()
    {
        // Activamos el objeto seleccionado
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }

        // Desactivamos los otros 3 objetos
        if (objectToDeactivate1 != null)
        {
            objectToDeactivate1.SetActive(false);
        }
        if (objectToDeactivate2 != null)
        {
            objectToDeactivate2.SetActive(false);
        }
        if (objectToDeactivate3 != null)
        {
            objectToDeactivate3.SetActive(false);
        }
    }
}