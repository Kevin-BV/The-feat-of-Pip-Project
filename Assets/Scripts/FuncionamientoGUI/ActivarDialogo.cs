using UnityEngine;
using System.Collections;

public class ActivarDialogo : MonoBehaviour
{
    [SerializeField] private string firstDialogText;  // Primer diálogo
    [SerializeField] private string secondDialogText; // Segundo diálogo
    [SerializeField] private DialogManager dialogManager; // Referencia al DialogManager
    [SerializeField] private float dialogDuration = 6f; // Tiempo en segundos para ocultar el diálogo

    private bool isPlayerInTrigger = false; // Verifica si el jugador está dentro del trigger
    private Coroutine dialogCoroutine; // Referencia a la coroutine en ejecución

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que el jugador tenga la etiqueta "Player"
        {
            isPlayerInTrigger = true;
            if (dialogCoroutine != null)
            {
                StopCoroutine(dialogCoroutine); // Detén la coroutine anterior si está en ejecución
            }
            dialogCoroutine = StartCoroutine(ShowDialogSequence()); // Comienza la secuencia de los diálogos
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
        // Mostrar el primer diálogo
        dialogManager.ShowDialog(firstDialogText);

        // Espera 5 segundos antes de mostrar el siguiente diálogo
        yield return new WaitForSeconds(3f);

        // Si el jugador aún está dentro del trigger, mostrar el segundo diálogo
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

        // Si el jugador ya no está en el trigger, ocultamos el diálogo
        if (!isPlayerInTrigger)
        {
            dialogManager.HideDialog();
        }
        else
        {
            // Si el tiempo se cumplió y el jugador sigue dentro del trigger, ocultamos el diálogo
            dialogManager.HideDialog();
        }
    }
}
