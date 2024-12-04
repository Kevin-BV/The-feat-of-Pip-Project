using UnityEngine;
using TMPro; // Aseg�rate de importar TextMeshPro

public class DialogManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogUI; // Referencia al panel del di�logo
    [SerializeField] private TMP_Text dialogTextComponent; // Referencia al componente TextMeshPro

    private void Start()
    {
        dialogUI.SetActive(false); // Aseg�rate de que el di�logo est� oculto al inicio
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
