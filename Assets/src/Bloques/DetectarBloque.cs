using UnityEngine;

public enum TipoContacto
    {
        ContactoNormal,
        ContactoPosicional
    }

public class DetectarBloque : MonoBehaviour
{
    [SerializeField] private GameObject bloqueInContact;

    [SerializeField] private TipoContacto tipoContacto;
    private int index;
    public TipoContacto GetTipoContacto(){
        return tipoContacto;
    }

    public int GetIndex(){
        return index;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("BloqueGeneral")){ 
            if (other.transform.gameObject.TryGetComponent<BloqueArrastrable>(out BloqueArrastrable bloque))
            {
                if(bloqueInContact != null){
                    SetNullBloqueInContact();
                }
                bloqueInContact = other.gameObject;
                bloque.Brillar();
                tipoContacto = TipoContacto.ContactoNormal;
            }
        }else if(other.gameObject.layer == LayerMask.NameToLayer("BloquePosicional")){
            if (other.transform.parent.gameObject.TryGetComponent<BloqueArrastrable>(out BloqueArrastrable bloque))
            {
                if(bloqueInContact != null){
                    SetNullBloqueInContact();
                }
                bloqueInContact = other.gameObject;
                bloque.Brillar();
                tipoContacto = TipoContacto.ContactoPosicional;
                index = GetLastNumber(other.gameObject.name);
            }
        }
    }

    int GetLastNumber(string name)
    {
        var match = System.Text.RegularExpressions.Regex.Match(name, @"\d+$");
        if (match.Success)
        {
            return int.Parse(match.Value); 
        }
        else
        {
            Debug.LogError("No se encontró un número al final del nombre.");
            return -1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("BloqueGeneral")){        
            if (other.transform.gameObject.TryGetComponent<BloqueArrastrable>(out BloqueArrastrable bloque))
            {
                if (bloqueInContact == other.gameObject)
                {
                    SetNullBloqueInContact();              
                }
            }
        }else if(other.gameObject.layer == LayerMask.NameToLayer("BloquePosicional")){
            if (other.transform.parent.gameObject.TryGetComponent<BloqueArrastrable>(out BloqueArrastrable bloque))
            {
                if (bloqueInContact == other.gameObject)
                {
                    SetNullBloqueInContact();              
                }
            }
        }
    }

    private void SetNullBloqueInContact()
    {
        if (bloqueInContact == null) return;

        BloqueArrastrable bloque = bloqueInContact.GetComponent<BloqueArrastrable>();
        if (bloque == null && bloqueInContact.transform.parent != null)
        {
            bloque = bloqueInContact.transform.parent.GetComponent<BloqueArrastrable>();
        }
        if (bloque != null)
        {
            bloque.NoBrillar();
        }
        bloqueInContact = null;
    }


    public GameObject GetBloqueEnContacto()
    {
        if(bloqueInContact == null){return null;}
        GameObject bloque = bloqueInContact;
        SetNullBloqueInContact();
        return bloque;
    }
}
