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
            if (nivel != null)
            {
                nivel.Win(); 
            }
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("lose")){
            if (nivel != null)
            {
                nivel.Lose(); 
            }
        }
    }

}
