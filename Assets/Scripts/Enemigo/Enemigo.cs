using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public int salud = 50;

    public void TomarDaño(int cantidad)
    {
        salud -= cantidad;

        if (salud <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        Destroy(gameObject); // Destruye al enemigo
    }

  
}