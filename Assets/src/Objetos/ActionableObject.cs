using UnityEngine;

public class ActionableObject : MonoBehaviour
{
    private MovableObject movebleObject;
    private RotatableObject rotatableObject;
    private JumpableObject jumpableObject;
    private ScalableObject scalableObject;
    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;
    private Vector3 initialLocalScale;

    private bool hasMoved = false;

    private void Awake()
    {
        movebleObject = gameObject.AddComponent<MovableObject>();
        rotatableObject = gameObject.AddComponent<RotatableObject>();
        jumpableObject = gameObject.AddComponent<JumpableObject>();
        scalableObject = gameObject.AddComponent<ScalableObject>();
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;
        initialLocalScale = transform.localScale;
    }

    public ProgramableObject GetProgramableObject()
    {
        return GetComponent<ProgramableObject>();
    }

    public bool HasMoved()
    {
        return hasMoved;
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
        transform.localScale = initialLocalScale;
        hasMoved = false;
    }
    public void MoveForward()
    {
        hasMoved = true;
        if (!movebleObject.IsMoving())
        {
            movebleObject.MoveForward();
        }
    }
    public void RotateRight()
    {
        hasMoved = true;
        if (!rotatableObject.IsRotating())
        {
            rotatableObject.StartRotation(transform.rotation, transform.rotation * Quaternion.Euler(0, 90f, 0));
        }
    }

    public void RotateLeft()
    {
        hasMoved = true;
        if (!rotatableObject.IsRotating())
        {
            rotatableObject.StartRotation(transform.rotation, transform.rotation * Quaternion.Euler(0, -90f, 0));
        }
    }

    public void Jump()
    {
        hasMoved = true;
        jumpableObject.Jump();
    }

    public void Agrandar()
    {
        hasMoved = true;
        scalableObject.Scale(true);
    }

    public void Reducir()
    {
        hasMoved = true;
        scalableObject.Scale(false);
    }

    public bool IsMoving()
    {
        return movebleObject.IsMoving() || rotatableObject.IsRotating() || jumpableObject.IsJumping() || scalableObject.IsScaling();
    }

    public void Stop()
    {
        movebleObject.Stop();
        rotatableObject.Stop();
        jumpableObject.Stop();
        scalableObject.Stop();
    }
}
