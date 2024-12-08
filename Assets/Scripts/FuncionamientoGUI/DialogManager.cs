using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogUI; // Referencia al panel del diálogo
    [SerializeField] private Text FuenteTexto;

    private void Start()
    {
        dialogUI.SetActive(false); // Asegúrate de que el diálogo esté oculto al inicio
    }

    public void ShowDialog(string dialogText)
    {
        FuenteTexto.text = dialogText;
        dialogUI.SetActive(true);
    }

    public void HideDialog()
    {
        dialogUI.SetActive(false);
    }
}
