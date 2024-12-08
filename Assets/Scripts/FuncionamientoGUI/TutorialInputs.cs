using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialInputs : MonoBehaviour
{
    [Header("Imagen a activar")]
    public Image targetImage; // La imagen que quieres activar desde el inspector

    private void Start()
    {
        if (targetImage != null)
        {
            targetImage.gameObject.SetActive(false); // Asegurarte de que la imagen está desactivada al inicio
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica si el objeto tiene el tag "Player"
        {
            if (targetImage != null)
            {
                targetImage.gameObject.SetActive(true); // Activa la imagen
            }
            else
            {
                Debug.LogWarning("No se ha asignado ninguna imagen en el inspector.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica si el objeto tiene el tag "Player"
        {
            if (targetImage != null)
            {
                targetImage.gameObject.SetActive(false); // Desactiva la imagen
            }
        }
    }
}