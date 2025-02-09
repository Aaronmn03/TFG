using UnityEngine;
using UnityEngine.EventSystems;

public class ZonaBloques: MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject draggedObject = eventData.pointerDrag;
        if (draggedObject != null && draggedObject.GetComponent<Bloque>() != null)
        {
            draggedObject.GetComponent<Bloque>().DeleteBloque();
        }
    }
}
