using UnityEngine;

public class MovableObject : MonoBehaviour
{
    public float moveSpeed = 0.075f;
    private Vector3 startPosition, destination;
    private float journeyLength, moveStartTime;
    private bool isMoving = false;
    private float step = 0.1f;

    private AudioSource audioSource;
    
    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    public void MoveForward()
    {
        if (!isMoving)
        {
            StartMovement(transform.position, transform.position + transform.forward * step);
        }
    }

    public void StartMovement(Vector3 from, Vector3 to)
    {
        AnimatorHandlerPlayer animatorHandler = transform.GetChild(0).GetComponent<AnimatorHandlerPlayer>();
        animatorHandler.Walk();
        audioSource.Play();

        startPosition = from;
        destination = to;
        journeyLength = Vector3.Distance(from, to);
        moveStartTime = Time.time;
        isMoving = true;
    }

    public void Stop()
    {
        isMoving = false;
        transform.GetChild(0).GetComponent<AnimatorHandlerPlayer>().StopWalk();
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            isMoving = UpdateMovement();
        }
    }

    private bool UpdateMovement()
    {
        float progress = GetProgress(moveStartTime, journeyLength, moveSpeed);
        Vector3 newPosition = Vector3.Lerp(startPosition, destination, progress);
        rb.MovePosition(newPosition);

        if (progress >= 1.0f)
        {
            rb.MovePosition(destination);
            transform.GetChild(0).GetComponent<AnimatorHandlerPlayer>().StopWalk();
            audioSource.Pause();
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
