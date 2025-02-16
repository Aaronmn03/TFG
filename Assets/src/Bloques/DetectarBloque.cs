using UnityEngine;

public class DetectarBloque : MonoBehaviour
{
    private GameObject bloqueInContact;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.TryGetComponent<BloqueArrastrable>(out BloqueArrastrable bloque))
        {
            bloqueInContact = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject.TryGetComponent<BloqueArrastrable>(out BloqueArrastrable bloque))
        {
            if (bloqueInContact == other.gameObject)
            {
                bloqueInContact = null;
            }
        }
    }

    public Bloque GetBloqueEnContacto()
    {
        if(bloqueInContact == null){return null;}
        return bloqueInContact.GetComponent<Bloque>();
    }
}
