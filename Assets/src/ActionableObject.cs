using UnityEngine;

public class ActionableObject : MonoBehaviour
{
    public float speed = 1.5f; // Velocidad máxima
    private Vector3 startPosition;
    private Vector3 destination;
    private float journeyLength;
    private float startTime;
    private bool isMoving = false;

    public void MoveForward()
    {
        if (!isMoving)
        {
            startPosition = transform.position;
            destination = startPosition + new Vector3(0, 0, 0.6f);
            journeyLength = Vector3.Distance(startPosition, destination);
            startTime = Time.time;
            isMoving = true;
        }
    }

    void Update()
    {
        if (isMoving)
        {
            float distCovered = (Time.time - startTime) * speed;
            float t = distCovered / journeyLength; 

            t = Mathf.SmoothStep(0, 1, t); 

            transform.position = Vector3.Lerp(startPosition, destination, t);

            if (t >= 1.0f)
            {
                isMoving = false;
                transform.position = destination;
            }
        }
    }
}
