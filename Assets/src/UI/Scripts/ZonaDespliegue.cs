using UnityEngine;
using UnityEngine.EventSystems;

public class ZonaDespliegue : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject draggedObject = eventData.pointerDrag;
        if (draggedObject != null && draggedObject.GetComponent<BloqueArrastrable>() != null)
        {
            draggedObject.transform.SetParent(transform);
        }
    }
}
