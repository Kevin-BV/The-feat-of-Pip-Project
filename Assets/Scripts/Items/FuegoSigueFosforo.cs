using UnityEngine;

public class FuegoSigueFosforo : MonoBehaviour
{
    public Transform torchTip; // Asigna aqu� la punta de la antorcha desde el inspector.

    void Update()
    {
        if (torchTip != null)
        {
            transform.position = torchTip.position;
        }
    }
}
