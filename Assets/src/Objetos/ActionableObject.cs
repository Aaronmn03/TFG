using UnityEngine;

public class ActionableObject : MonoBehaviour
{
    private MovableObject movebleObject;
    private RotatableObject rotatableObject;

    private JumpableObject jumpableObject;
    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;

    private void Awake()
    {
        movebleObject = gameObject.AddComponent<MovableObject>();
        rotatableObject = gameObject.AddComponent<RotatableObject>();
        jumpableObject = gameObject.AddComponent<JumpableObject>();
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;
    }
    public void ResetObject()
    {
        Rigidbody rb = GetComponent<Rigidbody>();        
        if (rb != null)
        {
            rb.velocity = Vector3.zero; 
            rb.angularVelocity = Vector3.zero;
            rb.Sleep();
        }
        transform.localPosition = initialLocalPosition;
        transform.localRotation = initialLocalRotation;
    }
    public void MoveForward()
    {
        if (!movebleObject.IsMoving())
        {
            movebleObject.MoveForward();
        }
    }
    public void RotateRight()
    {
        if (!rotatableObject.IsRotating())
        {
            rotatableObject.StartRotation(transform.rotation, transform.rotation * Quaternion.Euler(0, 90f, 0));
        }
    }

    public void RotateLeft()
    {
        if (!rotatableObject.IsRotating())
        {
            rotatableObject.StartRotation(transform.rotation, transform.rotation * Quaternion.Euler(0, -90f, 0));
        }
    }

    public void Jump()
    {
        jumpableObject.Jump();
    }

    public bool IsMoving()
    {
        return movebleObject.IsMoving() || rotatableObject.IsRotating() || jumpableObject.IsJumping();
    }
}
