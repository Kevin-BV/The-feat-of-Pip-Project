using UnityEngine;
using UnityEngine.SceneManagement; 

public class CambiarEscenaTrigger : MonoBehaviour
{
    [SerializeField]
    private string sceneName; 

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            
            SceneManager.LoadScene(sceneName);
        }
    }
}
