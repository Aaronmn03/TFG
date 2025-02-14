using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ZonaProgramacion : MonoBehaviour
{
    private void Start() {
        NonSelectedObject();
    }
    public void NonSelectedObject(){
        GetComponent<MeshRenderer>().enabled = false;
        HideBloques(gameObject);
    }

    public void SelectedObject(){
        GetComponent<MeshRenderer>().enabled = true;
        ShowBloques(gameObject);
    }

    private void HideBloques(GameObject parent)
    {
        SetChildrenVisibility(parent, false);
    }

    private void ShowBloques(GameObject parent)
    {
        SetChildrenVisibility(parent, true);
    }
    private void SetChildrenVisibility(GameObject parent, bool visible)
    {
        if (parent == null)
        {
            Debug.LogWarning("SetChildrenVisibility: parent is null.");
            return;
        }
        
        foreach (Transform child in parent.transform)
        {
            Bloque bloque = child.GetComponent<Bloque>();
            if (bloque != null)
            {
                MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.enabled = visible;
                }
            }
        }
    }
}
