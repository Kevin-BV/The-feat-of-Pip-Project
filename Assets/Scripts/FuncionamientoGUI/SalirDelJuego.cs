using UnityEngine;
using UnityEngine.UI; 

public class SalirDelJuego : MonoBehaviour
{
    public void ExitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}