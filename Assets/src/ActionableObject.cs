using UnityEngine;

public class ActionableObject : MonoBehaviour
{
    // Variables para el movimiento
    public float moveSpeed = 0.3f;
    private Vector3 startPosition;
    private Vector3 destination;
    private float journeyLength;
    private float moveStartTime;
    private bool isMoving = false;

    // Variables para la rotación
    public float rotationSpeed = 90f; // grados por segundo
    private Quaternion startRotation;
    private Quaternion destinationRotation;
    private float rotationStartTime;
    private bool isRotating = false;
    public void MoveForward()
    {
        if (!isMoving)
        {
            startPosition = transform.position;
            destination = startPosition + new Vector3(0, 0, 0.6f);
            journeyLength = Vector3.Distance(startPosition, destination);
            moveStartTime = Time.time;
            isMoving = true;
        }
    }

    public void RotateRight()
    {
        if (!isRotating)
        {
            startRotation = transform.rotation;
            destinationRotation = transform.rotation * Quaternion.Euler(0, 90f, 0);
            rotationStartTime = Time.time;
            isRotating = true;
        }
    }
    void Update()
    {
        if (isMoving)
        {
            float distCovered = (Time.time - moveStartTime) * moveSpeed;
            float t = distCovered / journeyLength; 

            t = Mathf.SmoothStep(0, 1, t); 

            transform.position = Vector3.Lerp(startPosition, destination, t);

            if (t >= 1.0f)
            {
                isMoving = false;
                transform.position = destination;
            }
        }
        if (isRotating)
        {
            float angleCovered = (Time.time - rotationStartTime) * rotationSpeed;
            float t = angleCovered / 90f; // 90° es la rotación total deseada
            t = Mathf.SmoothStep(0, 1, t);
            transform.rotation = Quaternion.Slerp(startRotation, destinationRotation, t);

            if (t >= 1.0f)
            {
                isRotating = false;
                transform.rotation = destinationRotation;
            }
        }
    }
}
