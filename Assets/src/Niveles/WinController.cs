using UnityEngine;

public class WinController : MonoBehaviour
{
    private Nivel nivel;

    private void Start() {
        nivel = FindObjectOfType<Nivel>();
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("win"))
        {
            Debug.Log("Trigger");
            if (nivel != null)
            {
                nivel.Win(); 
            }
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("lose")){
            Debug.Log("Collider");
            if (nivel != null)
            {
                nivel.Lose(); 
            }
        }
    }

}
