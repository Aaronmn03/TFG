using UnityEngine;

public class MovableObject : MonoBehaviour
{
    public float moveSpeed = 0.3f;
    private Vector3 startPosition, destination;
    private float journeyLength, moveStartTime;
    private bool isMoving = false;

    public void MoveForward()
    {
        if (!isMoving)
        {
            StartMovement(transform.position, transform.position + transform.forward * 0.6f);
        }
    }

    public void StartMovement(Vector3 from, Vector3 to)
    {
        startPosition = from;
        destination = to;
        journeyLength = Vector3.Distance(from, to);
        moveStartTime = Time.time;
        isMoving = true;
    }

    private void Update()
    {
        if (isMoving)
        {
            isMoving = UpdateMovement();
        }
    }

    private bool UpdateMovement()
    {
        float progress = GetProgress(moveStartTime, journeyLength, moveSpeed);
        transform.position = Vector3.Lerp(startPosition, destination, progress);

        if (progress >= 1.0f)
        {
            transform.position = destination;
            return false;
        }
        return true;
    }

    private float GetProgress(float startTime, float totalDistance, float speed)
    {
        float covered = (Time.time - startTime) * speed;
        return Mathf.SmoothStep(0, 1, covered / totalDistance);
    }

    public bool IsMoving() => isMoving;
}