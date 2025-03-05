using UnityEngine;

public class ZoomYMovimiento : MonoBehaviour
{
    public float zoomSpeed = 0.1f;  
    private float touchDistanciaPrev = 0f;
    public Vector3 escalaMinima = new Vector3(0.0015f, 0.00099f, 0.00099f);
    public Vector3 escalaMaxima;

    void Update()
    {
        if (Input.touchCount == 2)
        {
            ProcesarZoomTouch();
        }
        #if UNITY_EDITOR
        ProcesarZoomEditor();
        #endif
    }

    // --- MÓVIL: Zoom con 2 dedos ---
    private void ProcesarZoomTouch()
    {
        Touch touch0 = Input.GetTouch(0);
        Touch touch1 = Input.GetTouch(1);
        if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
        {
            touchDistanciaPrev = (touch0.position - touch1.position).magnitude;
            return;
        }
        float distanciaActual = (touch0.position - touch1.position).magnitude;
        float distanciaDelta = distanciaActual - touchDistanciaPrev;
        Vector3 nuevaEscala = transform.localScale + Vector3.one * distanciaDelta * zoomSpeed;
        transform.localScale = Vector3.Max(escalaMinima, Vector3.Min(escalaMaxima, nuevaEscala));
        touchDistanciaPrev = distanciaActual;
    }

    // --- EDITOR UNITY: Simulación con Mouse ---
    private void ProcesarZoomEditor()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            Vector3 nuevaEscala = transform.localScale + Vector3.one * scroll * zoomSpeed * 50;
            transform.localScale = Vector3.Max(escalaMinima, Vector3.Min(escalaMaxima, nuevaEscala));
        }
    }
}
