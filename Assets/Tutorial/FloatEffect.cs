using UnityEngine;

public class FloatEffect : MonoBehaviour
{
    public float floatSpeed;
    public float floatHeight;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.position = startPosition + new Vector3(0, Mathf.Sin(Time.time * floatSpeed) * floatHeight, 0);
    }
}
