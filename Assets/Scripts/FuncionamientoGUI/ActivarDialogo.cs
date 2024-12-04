using UnityEngine;

public class ActivarDialogo : MonoBehaviour
{
    [SerializeField] private string dialogText; // Texto del diálogo
    [SerializeField] private DialogManager dialogManager; // Referencia al DialogManager

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que el jugador tenga la etiqueta "Player"
        {
            dialogManager.ShowDialog(dialogText);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogManager.HideDialog();
        }
    }
}
