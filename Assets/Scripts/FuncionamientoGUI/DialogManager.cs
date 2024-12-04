using UnityEngine;
using TMPro; // Asegúrate de importar TextMeshPro

public class DialogManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogUI; // Referencia al panel del diálogo
    [SerializeField] private TMP_Text dialogTextComponent; // Referencia al componente TextMeshPro

    private void Start()
    {
        dialogUI.SetActive(false); // Asegúrate de que el diálogo esté oculto al inicio
    }

    public void ShowDialog(string dialogText)
    {
        dialogTextComponent.text = dialogText;
        dialogUI.SetActive(true);
    }

    public void HideDialog()
    {
        dialogUI.SetActive(false);
    }
}
