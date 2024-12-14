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

    [Header("Objeto de referencia para mover los objetos")]
    public GameObject objectToMoveTo; // El objeto de referencia a donde se moverán los objetos

    private bool isInTriggerZone = false; // Para saber si el jugador está dentro del trigger
    private bool hasActivated = false;  // Para saber si ya se activaron los objetos

    private void Update()
    {
        // Solo activamos y movemos los objetos si estamos dentro del trigger y presionamos la tecla X
        if (isInTriggerZone && !hasActivated && Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Tecla X presionada dentro del trigger");
            MoveAndActivateObjects();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Cuando el jugador entra en la zona del trigger
        if (other.CompareTag("Player"))
        {
            isInTriggerZone = true;
            hasActivated = false;  // Restablecer la activación al entrar nuevamente al trigger
            Debug.Log("Jugador ha entrado en el trigger.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Cuando el jugador sale de la zona del trigger
        if (other.CompareTag("Player"))
        {
            isInTriggerZone = false;
            Debug.Log("Jugador ha salido del trigger.");
        }
    }

    private void MoveAndActivateObjects()
    {
        // Solo mover y activar los objetos si estamos dentro del trigger
        if (isInTriggerZone)
        {
            // Activamos el objeto seleccionado y movemos a la posición de referencia
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
                objectToActivate.transform.position = objectToMoveTo.transform.position;
                Debug.Log("Objeto activado y movido: " + objectToActivate.name);
            }

            // Desactivamos los otros 3 objetos y los movemos a la posición de referencia
            if (objectToDeactivate1 != null)
            {
                objectToDeactivate1.SetActive(false);
                objectToDeactivate1.transform.position = objectToMoveTo.transform.position;
                Debug.Log("Objeto desactivado y movido: " + objectToDeactivate1.name);
            }
            if (objectToDeactivate2 != null)
            {
                objectToDeactivate2.SetActive(false);
                objectToDeactivate2.transform.position = objectToMoveTo.transform.position;
                Debug.Log("Objeto desactivado y movido: " + objectToDeactivate2.name);
            }
            if (objectToDeactivate3 != null)
            {
                objectToDeactivate3.SetActive(false);
                objectToDeactivate3.transform.position = objectToMoveTo.transform.position;
                Debug.Log("Objeto desactivado y movido: " + objectToDeactivate3.name);
            }

            // Marcar que ya se ha activado
            hasActivated = true;
        }
        else
        {
            Debug.Log("No se puede mover ni activar objetos fuera del trigger.");
        }
    }
}