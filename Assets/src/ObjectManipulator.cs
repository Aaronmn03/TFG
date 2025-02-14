using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 

public class ObjectManipulator : MonoBehaviour
{
    public GameObject ARObject;
    public BloqueArrastrable bloqueObject;
    [SerializeField] private Camera ARCamera;
    private bool isARObjectSelected;
    private bool isARBloqueSelected;
    private string selectedObjectTag = "ARObject";
    private Vector2 touchPosition;

    private Vector2 firstInput = Vector2.zero;

    [SerializeField] private float ScreenFactor = 0.8f;


    public bool GetIsARObjectSelected(){
        return isARObjectSelected;
    }
    public ProgramableObject GetProgramableObject(){
        return ARObject.GetComponent<ProgramableObject>();
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
            if (Input.GetMouseButton(0) && bloqueObject != null)
            {
                touchPosition = Input.mousePosition;
                Vector2 diff = (firstInput - touchPosition) * ScreenFactor;
                Bloque bloque = bloqueObject.GetBloque();
                if (bloque.HasParent())
                {
                    bloque.GetParent().UnConnectTo(bloque);
                }
                bloqueObject.Move(diff);
                firstInput = touchPosition;
            }
            if (Input.GetMouseButtonUp(0) && bloqueObject != null)
            {
                bloqueObject.OnEndDrag();
                bloqueObject = null;
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
                        if (firstTouch.phase == TouchPhase.Moved && bloqueObject != null)
                        {
                            touchPosition = firstTouch.position;
                            Vector2 diff = (firstInput - touchPosition) * ScreenFactor;
                            Bloque bloque = bloqueObject.GetBloque();
                            if (bloque.HasParent())
                            {
                                bloque.GetParent().UnConnectTo(bloque);
                            }
                            bloqueObject.Move(diff);
                            firstInput = touchPosition;
                        }
                        if (firstTouch.phase == TouchPhase.Ended && bloqueObject != null)
                        {
                            bloqueObject.OnEndDrag();
                            bloqueObject = null;
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
            }
        }
        return false;
    }

    private void SelectObject(GameObject hit){
        Debug.Log("Queremos mover el:" + hit.transform.gameObject.name);        
        if (hit.transform.gameObject.TryGetComponent<ProgramableObject>(out ProgramableObject ProgramableObject))
        {
            ARObject = hit.transform.gameObject;
            ProgramableObject.SelectObject();
            
        }
        if (hit.transform.gameObject.TryGetComponent<BloqueArrastrable>(out BloqueArrastrable bloque))
        {
            bloqueObject = bloque;  
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
        }
        else
        {
            Debug.LogWarning("El objeto seleccionado no es ni ProgramableObject ni Bloque.");
        }
        ARObject = null;
    }
}
