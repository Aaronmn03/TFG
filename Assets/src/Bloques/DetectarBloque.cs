using UnityEngine;

public enum TipoContacto
    {
        ContactoNormal,
        ContactoPosicional,
        ContactoInterno,
    }

public class DetectarBloque : MonoBehaviour
{
    [SerializeField] private GameObject bloqueInContact;

    [SerializeField] private TipoContacto tipoContacto;
    private int index;

    public bool isInBasura = false;
    public TipoContacto GetTipoContacto(){
        return tipoContacto;
    }

    public int GetIndex(){
        return index;
    }

    public bool IsInBasura(){
        return isInBasura;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;
        if(other.gameObject.tag == "Basura") {
            other.gameObject.GetComponent<Animator>().SetTrigger("use");
            bloqueInContact = other.gameObject;
            isInBasura = true;
            return;
        }
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("BloqueGeneral"))
        {
            ProcesarBloqueGeneral(other.gameObject);
        }
        else if (layer == LayerMask.NameToLayer("BloquePosicional"))
        {
            ProcesarBloquePosicional(other.gameObject);
        }
        else if (layer == LayerMask.NameToLayer("BloqueInterno"))
        {
            ProcesarBloqueInterno(other.gameObject);
        }
    }

    private void ProcesarBloqueGeneral(GameObject other)
    {
        if (!other.TryGetComponent(out BloqueArrastrable bloque)) return;
        if (GetComponent<Bloque>().GetBloqueControl() == bloque.GetComponent<Bloque>()) return;
        if (!GetComponent<Bloque>().isConectable(other.GetComponent<Bloque>(), TipoContacto.ContactoNormal)){return;}
        ActualizarBloqueEnContacto(other, bloque);
        if (bloque.GetBloque().HasBloqueControl())
        {
            bloque.GetBloque().GetBloqueControl().IncrementarTamano(GetComponent<Bloque>().GetBloquesConectados().Count + 1);
        }
        tipoContacto = TipoContacto.ContactoNormal;
    }

    private void ProcesarBloquePosicional(GameObject other)
    {
        GameObject parentObj = other.transform.parent?.gameObject;
        if (parentObj == null || !parentObj.TryGetComponent(out BloqueArrastrable bloque)) return;
        if (!GetComponent<Bloque>().isConectable(bloque.GetBloque(), TipoContacto.ContactoPosicional)){return;}

        ActualizarBloqueEnContacto(other, bloque);
        tipoContacto = TipoContacto.ContactoPosicional;
        index = GetLastNumber(other.name);
    }

    private void ProcesarBloqueInterno(GameObject other)
    {
        GameObject parentObj = other.transform.parent?.gameObject;
        if (parentObj == null || !parentObj.TryGetComponent(out BloqueArrastrable bloque)) return;
        if (!GetComponent<Bloque>().isConectable(bloque.GetBloque(), TipoContacto.ContactoInterno)){return;}

        ActualizarBloqueEnContacto(other, bloque);
        bloque.GetComponent<BloqueControl>().IncrementarTamano(GetComponent<Bloque>().GetBloquesConectados().Count + 1);
        tipoContacto = TipoContacto.ContactoInterno;
    }

    private void ActualizarBloqueEnContacto(GameObject newBloque, BloqueArrastrable bloque)
    {
        if (bloqueInContact != null) SetNullBloqueInContact();
        bloqueInContact = newBloque;
        bloque.Brillar();
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

    private void OnTriggerExit(Collider other){
        if (!enabled) return;

        if(other.gameObject.tag == "Basura") {
            other.gameObject.GetComponent<Animator>().SetTrigger("use");
            isInBasura = false;
            return;
        }

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
        }else if(other.gameObject.layer == LayerMask.NameToLayer("BloqueInterno")){
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
            if(bloque.TryGetComponent<BloqueControl>(out BloqueControl bloqueControl)){
                bloqueControl.IncrementarTamano(0);
            }
            if(bloque.GetBloque().HasBloqueControl()){
                bloque.GetBloque().GetBloqueControl().IncrementarTamano(0);
            }
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
