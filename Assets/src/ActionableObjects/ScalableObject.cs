using System.Collections;
using UnityEngine;

public class ScalableObject : MonoBehaviour
{
    private Vector3 startScale, targetScale;
    private float journeyLength, startTime;
    private bool isScaling = false;
    public float scaleSpeed = 0.1f;

    public void Scale(bool crecer)
    {
        if (!isScaling)
        {
            if(crecer){
                StartScaling(transform.localScale, transform.localScale * 2);
            }else{
                StartScaling(transform.localScale, transform.localScale / 2);
            }
        }
    }

    public void Stop()
    {
        isScaling = false;
    }

    private void StartScaling(Vector3 from, Vector3 to)
    {
        startScale = from;
        targetScale = to;
        journeyLength = Vector3.Distance(from, to);
        startTime = Time.time;
        isScaling = true;
    }

    private void FixedUpdate()
    {
        if (isScaling)
        {
            isScaling = UpdateScaling();
        }
    }

    private bool UpdateScaling()
    {
        float progress = (Time.time - startTime) * scaleSpeed / journeyLength;
        transform.localScale = Vector3.Lerp(startScale, targetScale, progress);

        if (progress >= 1.0f)
        {
            transform.localScale = targetScale;
            return false;
        }

        return true;
    }

    public bool IsScaling() => isScaling;
}
