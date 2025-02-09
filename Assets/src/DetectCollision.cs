using UnityEngine;

public class DetectarColision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("win"))
        {
            Nivel nivel = FindObjectOfType<Nivel>();
            if (nivel != null)
            {
                nivel.Win(); 
            }
        }
    }
}
