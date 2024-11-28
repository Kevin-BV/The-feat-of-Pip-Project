using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 

public class CambioEscena : MonoBehaviour
{
    
    public void CambiarEscena(string sceneName)
    {
        SceneManager.LoadScene(sceneName); 
    }
}
