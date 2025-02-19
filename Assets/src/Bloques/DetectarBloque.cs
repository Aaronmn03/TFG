using UnityEngine;

public class DetectarBloque : MonoBehaviour
{
    private GameObject bloqueInContact;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.TryGetComponent<BloqueArrastrable>(out BloqueArrastrable bloque))
        {
            if(bloqueInContact != null){
                SetNullBloqueInContact();
            }
            bloqueInContact = other.gameObject;
            bloqueInContact.GetComponent<BloqueArrastrable>().Brillar();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject.TryGetComponent<BloqueArrastrable>(out BloqueArrastrable bloque))
        {
            if (bloqueInContact == other.gameObject)
            {
                SetNullBloqueInContact();              
            }
        }
    }

    private void SetNullBloqueInContact(){
        bloqueInContact.GetComponent<BloqueArrastrable>().NoBrillar();
        bloqueInContact = null;       
    }

    public Bloque GetBloqueEnContacto()
    {
        if(bloqueInContact == null){return null;}
        return bloqueInContact.GetComponent<Bloque>();
        SetNullBloqueInContact();
    }
}
