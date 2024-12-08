using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogUI; // Referencia al panel del di�logo
    [SerializeField] private Text FuenteTexto;

    private void Start()
    {
        dialogUI.SetActive(false); // Aseg�rate de que el di�logo est� oculto al inicio
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
