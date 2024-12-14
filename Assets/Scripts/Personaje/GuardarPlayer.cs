using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardarPlayer : MonoBehaviour
{
    [Header("Objetos hijos")]
    public GameObject[] objetosHijos; // Los 3 objetos hijos que el script controlará

    private void Start()
    {
        // Al comenzar, recuperamos el último objeto activado desde PlayerPrefs
        CargarEstado();
    }

    private void OnDestroy()
    {
        // Antes de destruirse (al cambiar de escena), guardamos el estado del objeto activado
        GuardarEstado();
    }

    // Guarda el estado del objeto activado en PlayerPrefs
    private void GuardarEstado()
    {
        for (int i = 0; i < objetosHijos.Length; i++)
        {
            if (objetosHijos[i].activeSelf)
            {
                // Guardamos el índice del objeto activado
                PlayerPrefs.SetInt("ObjetoActivado", i);
                PlayerPrefs.Save();  // Guardamos en PlayerPrefs
                Debug.Log("Objeto activado guardado: " + objetosHijos[i].name);
                break;
            }
        }
    }

    // Carga el estado del objeto activado desde PlayerPrefs
    private void CargarEstado()
    {
        int indiceObjetoActivado = PlayerPrefs.GetInt("ObjetoActivado", -1);

        // Si el índice es válido, activamos el objeto correspondiente
        if (indiceObjetoActivado >= 0 && indiceObjetoActivado < objetosHijos.Length)
        {
            // Primero desactivamos todos los objetos hijos
            foreach (GameObject objeto in objetosHijos)
            {
                objeto.SetActive(false);
            }

            // Luego activamos el objeto correspondiente
            objetosHijos[indiceObjetoActivado].SetActive(true);
            Debug.Log("Objeto activado cargado: " + objetosHijos[indiceObjetoActivado].name);
        }
    }
}