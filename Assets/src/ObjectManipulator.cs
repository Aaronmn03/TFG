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

    private Vector2 firstInput = Vector2.zero;

    private ZonaBloques zonaBloques;

    private ZonaDespliegue zonaDespliegue;

    [SerializeField] private float ScreenFactor = 0.001f;

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
        #if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                firstInput = Input.mousePosition;
                touchPosition = firstInput;
                isARObjectSelected = CheckTouchOnARObject(touchPosition);
            }
            if (Input.GetMouseButton(0) && isARObjectSelected)
            {
                touchPosition = Input.mousePosition;
                Vector2 diff = (firstInput - touchPosition) * ScreenFactor;
                ARObject.transform.position = ARObject.transform.position - new Vector3(diff.x, diff.y, 0);
                firstInput = touchPosition;
            }
        #else
            if (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject())
                {
                    Touch firstTouch = Input.GetTouch(0);
                    if (Input.touchCount == 1)
                    {
                        if (firstTouch.phase == TouchPhase.Began)
                        {
                            firstInput = firstTouch.position;
                            touchPosition = firstTouch.position;
                            isARObjectSelected = CheckTouchOnARObject(touchPosition);
                        }
                        if (firstTouch.phase == TouchPhase.Moved && isARObjectSelected)
                        {
                            touchPosition = firstTouch.position;
                            Vector2 diff = (firstInput - touchPosition) * ScreenFactor;
                            ARObject.transform.position = ARObject.transform.position - new Vector3(diff.x, diff.y, 0);
                            firstInput = touchPosition;
                        }
                    }
                }
        #endif
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
                    return false;
                }
            }
        }
        return false;
    }

    private void SelectObject(GameObject hit){
        ARObject = hit.transform.gameObject;
        //Aqui habra que detectar si lo que estamos seleccionando es un bloque programable o un bloque
        if (ARObject.TryGetComponent<ProgramableObject>(out ProgramableObject ProgramableObject))
        {
            ProgramableObject.SelectObject();
            zonaBloques.SelectedObject();
            zonaDespliegue.SelectedObject();
        }
        else if (ARObject.TryGetComponent<BloquePrueba>(out BloquePrueba bloque))
        {
            //bloque.SelectBlock();
            Debug.Log("Bloque seleccionado");
        }
        else
        {
            Debug.LogWarning("El objeto seleccionado no es ni ProgramableObject ni Bloque.");
        }
    }

    private void DeselectObject(){
        if (ARObject.TryGetComponent<ProgramableObject>(out ProgramableObject ProgramableObject))
        {
            ProgramableObject.DeselectObject();
            zonaBloques.NonSelectedObject();
            zonaDespliegue.NonSelectedObject();
        }
        else if (ARObject.TryGetComponent<BloquePrueba>(out BloquePrueba bloque))
        {
            //bloque.DeselectBlock();
            Debug.Log("Bloque desseleccionado");
        }
        else
        {
            Debug.LogWarning("El objeto seleccionado no es ni ProgramableObject ni Bloque.");
        }
        ARObject = null;
    }
}
