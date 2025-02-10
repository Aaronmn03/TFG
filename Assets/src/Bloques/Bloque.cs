using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public abstract class Bloque : MonoBehaviour
{

    public List<Bloque> bloquesConectados = new List<Bloque>();

    public Bloque parent;
    
    public bool isConnect = false;   
    private Image bloqueImagen;
    private TextMeshProUGUI bloqueText;

    public abstract bool isConectable(Bloque other);
    public abstract IEnumerator Action();

    protected Nivel nivel;

    private void Start(){
        
        if (gameObject.GetComponent<BloqueArrastrable>() == null){
            gameObject.AddComponent<BloqueArrastrable>();
        }
        bloqueImagen = GetComponent<Image>();
        if (bloqueImagen == null)
        {
            Debug.LogError("No se encontró el componente Image en el objeto.");
        }
        bloqueText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (bloqueText == null)
        {
            Debug.LogError("No se encontró el componente TextMeshProUGUI en el primer hijo.");
        }
        nivel = GameObject.Find("LevelController").GetComponent<Nivel>();
    }

    public void SetColor(Color color)
    {
        bloqueImagen = GetComponent<Image>();
        if (bloqueImagen != null)
        {
            bloqueImagen.color = color;
            Debug.Log("cambiamos el color a: " + color.ToString());
        }
    }

    public void SetText(string texto, Color color)
    {
        bloqueText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (bloqueText != null)
        {
            bloqueText.text = texto;
            bloqueText.color = color;
        }
    }

    public void SetName(string name){
        gameObject.name = name;
    }

    public List<Bloque> getListConectados(){
        return bloquesConectados;
    }

    public Bloque GetParent(){
        return parent;
    }

    public void SetParent(Bloque value){
        parent = value;
    }

    public bool ConnectTo(Bloque parent){
        if(isConectable(parent)){
            List<Bloque> bloquesHijos = new List<Bloque>(parent.getListConectados());
            List<Bloque> bloquesConectar = new List<Bloque>(bloquesConectados);
            bloquesConectar.Insert(0, this);
            parent.AddBloques(0, bloquesConectar); 
            this.parent = parent;
            isConnect = true;
            if(bloquesHijos.Count != 0){
                foreach (Bloque bloque in this.bloquesConectados){
                    bloque.getListConectados().AddRange(bloquesHijos);
                }
                this.bloquesConectados.AddRange(bloquesHijos);
                if(this.HasChild()){
                    bloquesHijos[0].parent = bloquesConectar.Last();  
                }else{
                    bloquesHijos[0].parent = this;
                }
            }
            return true;
        }
        return false;       
    }

    public void UnConnectTo(Bloque child){  
        if (bloquesConectados.Contains(child)){
            RemoveBloque(child);
            isConnect = false;
            child.SetParent(null);
        }
    }

    public string toString(){
        return gameObject.name;
    }

    public void AddBloques(int index, List<Bloque> childs){
        bloquesConectados.InsertRange(index, childs);
        if (this.HasParent()){
            this.parent.AddBloques(index + 1, childs);
        }
    }

    public void RemoveBloque(Bloque child){
        List<Bloque> list = child.getListConectados();
        this.getListConectados().Remove(child);
        foreach(Bloque bloque_aux in list){
            this.getListConectados().Remove(bloque_aux);
        }
        if(this.HasParent()){
            this.parent.RemoveBloque(child);
        }

    }

    public bool HasParent(){
        return parent != null;
    }

    public bool HasChild(){
        return bloquesConectados.Count != 0;
    }

    public int NumChilds(){
        return bloquesConectados.Count;
    }
    
    public void DeleteBloque(){
        List<Bloque> list = GetComponent<Bloque>().getListConectados();
        foreach (Bloque bloque in list){
            Destroy(bloque.gameObject);
        }
        Destroy(this.gameObject);
    }
}
