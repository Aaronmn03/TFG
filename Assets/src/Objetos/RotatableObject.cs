using UnityEngine;
public class RotatableObject : MonoBehaviour
{
    public float rotationSpeed = 90f;
    private Quaternion startRotation, destinationRotation;
    private float rotationStartTime;
    private bool isRotating = false;

    public void RotateRight()
    {
        if (!isRotating)
        {
            StartRotation(transform.rotation, transform.rotation * Quaternion.Euler(0, 90f, 0));
        }
    }

    public void StartRotation(Quaternion from, Quaternion to)
    {
        startRotation = from;
        destinationRotation = to;
        rotationStartTime = Time.time;
        isRotating = true;
    }

    private void Update()
    {
        if (isRotating)
        {
            isRotating = UpdateRotation();
        }
    }

    private bool UpdateRotation()
    {
        float progress = GetProgress(rotationStartTime, 90f, rotationSpeed);
        transform.rotation = Quaternion.Slerp(startRotation, destinationRotation, progress);

        if (progress >= 1.0f)
        {
            transform.rotation = destinationRotation;
            return false;
        }
        return true;
    }

    private float GetProgress(float startTime, float totalDistance, float speed)
    {
        float covered = (Time.time - startTime) * speed;
        return Mathf.SmoothStep(0, 1, covered / totalDistance);
    }

    public bool IsRotating() => isRotating;
}