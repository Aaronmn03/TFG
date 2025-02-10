using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ZonaDespliegue : MonoBehaviour, IDropHandler
{
    private void Start() {
        NonSelectedObject();
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject draggedObject = eventData.pointerDrag;
        if (draggedObject != null && draggedObject.GetComponent<BloqueArrastrable>() != null)
        {
            draggedObject.transform.SetParent(transform);
            BloqueRaiz bloqueRaiz = draggedObject.GetComponent<BloqueRaiz>();
            if (bloqueRaiz != null)
            {
                bloqueRaiz.PutBloque();
            }
        }
    }
    public void NonSelectedObject(){
        Image image = gameObject.GetComponent<Image>();
        image.enabled = false;
    }

    public void SelectedObject(){
        Image image = gameObject.GetComponent<Image>();
        image.enabled = true;
    }
}
