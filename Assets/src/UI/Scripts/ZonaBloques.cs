using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ZonaBloques: MonoBehaviour, IDropHandler
{

    private void Start() {
        NonSelectedObject();
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject draggedObject = eventData.pointerDrag;
        if (draggedObject != null && draggedObject.GetComponent<Bloque>() != null)
        {
            BloqueRaiz bloqueRaiz = draggedObject.GetComponent<BloqueRaiz>();
            if (bloqueRaiz != null)
            {
                bloqueRaiz.RemoveBloque();
            }
            draggedObject.GetComponent<Bloque>().DeleteBloque();
        }
    }

    public void NonSelectedObject(){
        this.GetComponent<Image>().enabled = false;
        GameObject container = transform.GetChild(0).gameObject;
        container.GetComponent<Image>().enabled = false;
        Transform firstChild = container.transform.GetChild(0);
        Debug.Log(firstChild);
        foreach (Transform child in firstChild)
        {
            if (child != null)
            {
                Debug.Log(child);
                child.gameObject.SetActive(false);
            }
        }

    }

    public void SelectedObject(){
        this.GetComponent<Image>().enabled = true;
        GameObject container = transform.GetChild(0).gameObject;
        container.GetComponent<Image>().enabled = true;
        foreach (Transform child in container.transform.GetChild(0).transform)
        {
            if (child != null)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
