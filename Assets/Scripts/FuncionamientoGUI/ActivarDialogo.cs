using UnityEngine;
using System.Collections;

public class ActivarDialogo : MonoBehaviour
{
    [SerializeField] private string firstDialogText;  // Primer di�logo
    [SerializeField] private string secondDialogText; // Segundo di�logo
    [SerializeField] private DialogManager dialogManager; // Referencia al DialogManager
    [SerializeField] private float dialogDuration = 6f; // Tiempo en segundos para ocultar el di�logo

    private bool isPlayerInTrigger = false; // Verifica si el jugador est� dentro del trigger
    private Coroutine dialogCoroutine; // Referencia a la coroutine en ejecuci�n

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Aseg�rate de que el jugador tenga la etiqueta "Player"
        {
            isPlayerInTrigger = true;
            if (dialogCoroutine != null)
            {
                StopCoroutine(dialogCoroutine); // Det�n la coroutine anterior si est� en ejecuci�n
            }
            dialogCoroutine = StartCoroutine(ShowDialogSequence()); // Comienza la secuencia de los di�logos
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
        }
    }

    private IEnumerator ShowDialogSequence()
    {
        // Mostrar el primer di�logo
        dialogManager.ShowDialog(firstDialogText);

        // Espera 5 segundos antes de mostrar el siguiente di�logo
        yield return new WaitForSeconds(3f);

        // Si el jugador a�n est� dentro del trigger, mostrar el segundo di�logo
        if (isPlayerInTrigger)
        {
            dialogManager.ShowDialog(secondDialogText);
        }

        // Espera hasta que el tiempo se cumpla o el jugador salga del trigger
        float elapsedTime = 0f;
        while (elapsedTime < dialogDuration && isPlayerInTrigger)
        {
            elapsedTime += Time.deltaTime; // Sumar el tiempo transcurrido
            yield return null; // Espera hasta el siguiente frame
        }

        // Si el jugador ya no est� en el trigger, ocultamos el di�logo
        if (!isPlayerInTrigger)
        {
            dialogManager.HideDialog();
        }
        else
        {
            // Si el tiempo se cumpli� y el jugador sigue dentro del trigger, ocultamos el di�logo
            dialogManager.HideDialog();
        }
    }
}
