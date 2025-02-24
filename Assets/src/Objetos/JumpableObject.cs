using UnityEngine;

public class JumpableObject : MonoBehaviour
{
    public float moveSpeed = 0.075f;
    public float jumpHeight = 0.2f;

    public float jumpDuration = 0.5f;
    private Vector3 startPosition, destination;
    private float journeyLength, moveStartTime;
    private bool isJumping = false;
    private float step = 0.1f;
    
    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    public void Jump()
    {
        if (!isJumping)
        {
            StartJump(transform.position, transform.position + transform.forward * step);
        }
    }

    private void StartJump(Vector3 from, Vector3 to)
    {
        AnimatorHandlerPlayer animatorHandler = transform.GetChild(0).GetComponent<AnimatorHandlerPlayer>();
        animatorHandler.Jump(); 
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        startPosition = from;
        destination = to;
        journeyLength = Vector3.Distance(from, to);
        moveStartTime = Time.time;
        isJumping = true;
    }

    private void FixedUpdate()
    {
        if (isJumping)
        {
            isJumping = UpdateJump();
        }
    }

    private bool UpdateJump()
    {
        float progress = GetProgress(moveStartTime, journeyLength, moveSpeed);
        Vector3 newPosition = Vector3.Lerp(startPosition, destination, progress);
        float heightFactor = Mathf.Sin(progress * Mathf.PI) * jumpHeight;
        newPosition.y += heightFactor;
        rb.MovePosition(newPosition);
        if (progress >= 1.0f)
        {
            rb.MovePosition(destination);
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            //transform.GetChild(0).GetComponent<AnimatorHandlerPlayer>().Land(); 
            return false;
        }
        return true;
    }
    private float GetProgress(float startTime, float totalDistance, float speed)
    {
        float covered = (Time.time - startTime) * speed;
        return Mathf.Clamp01(covered / totalDistance);
    }

    public bool IsJumping() => isJumping;
}
