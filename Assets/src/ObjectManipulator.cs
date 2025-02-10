using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 

public class ObjectManipulator : MonoBehaviour
{
    public GameObject ARObject;
    [SerializeField] private Camera ARCamera;
    private bool isARObjectSelected;
    private string selectedObjectTag = "ARObject";
    private Vector2 touchPosition;

    private ZonaBloques zonaBloques;

    private ZonaDespliegue zonaDespliegue;

    public bool GetIsARObjectSelected(){
        return isARObjectSelected;
    }
    public ProgramableObject getARObject(){
        return ARObject.GetComponent<ProgramableObject>();
    }

    private void Start() {
        zonaBloques = GameObject.Find("Zona_programacion").GetComponent<ZonaBloques>();
        zonaDespliegue = GameObject.Find("AreaTrabajo").GetComponent<ZonaDespliegue>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 mousePos = Input.mousePosition;
            CheckTouchOnARObject(mousePos);
        }
    }
    private bool CheckTouchOnARObject(Vector2 touchPosition){
        Ray ray = ARCamera.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit)){
            if(hit.transform.tag == selectedObjectTag){
                if (ARObject != null && ARObject == hit.transform.gameObject)
                {
                    DeselectObject();
                    return false;
                }
                SelectObject(hit.transform.gameObject);
                return true;
            }else{
                if(ARObject != null){
                    DeselectObject();
                }
            }
        }
        return false;
    }

    private void SelectObject(GameObject hit){
        ARObject = hit.transform.gameObject;
        ARObject.GetComponent<ProgramableObject>().SelectObject();
        isARObjectSelected = true;
        zonaBloques.SelectedObject();
        zonaDespliegue.SelectedObject();
    }

    private void DeselectObject(){
        ARObject.GetComponent<ProgramableObject>().DeselectObject();
        ARObject = null;
        isARObjectSelected = false;
        zonaBloques.NonSelectedObject();
        zonaDespliegue.NonSelectedObject();
    }
}
