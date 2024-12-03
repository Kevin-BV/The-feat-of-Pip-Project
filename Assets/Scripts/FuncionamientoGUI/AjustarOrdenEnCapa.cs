using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(SortingGroup))]
public class AjustarOrdenPersonaje : MonoBehaviour
{
    public int offsetOrden = 0; // Ajuste manual para prioridad visual

    private SortingGroup sortingGroup;

    void Awake()
    {
        sortingGroup = GetComponent<SortingGroup>();
    }

    void LateUpdate()
    {
        // Ajustar el sortingOrder del Sorting Group según la posición Z
        sortingGroup.sortingOrder = -(int)(transform.position.z * 1000) + offsetOrden;
    }
}
