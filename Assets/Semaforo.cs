using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semaforo : ObtenedorVariable
{
    [SerializeField] private Transform rojo;
    [SerializeField] private Transform verde;
    [SerializeField] private Animator animator_ardilla;
    Coroutine subirBellota;
    private void Start() {
        base.Start();
        if (nivel != null) {
            nivel.PlayEvent += OnPlay; 
            nivel.ResetEvent += Resetear;
        }
    }

    private void Resetear(){
        valor = Color.clear;
        animator_ardilla.SetTrigger("saltar");
        if(subirBellota != null)
            StopCoroutine(subirBellota);
        ResetearBellota(rojo);
        ResetearBellota(verde);
    }

    private void OnPlay(){
        if (!this.enabled || !gameObject.activeInHierarchy){
            return;
        }
        List<Color> colors = new List<Color>(){Color.red, Color.green};        
        valor = colors[Random.Range(0, colors.Count)];
        if (valor is Color color && color == Color.red){    
            subirBellota = StartCoroutine(SubirBellota(rojo));
            StartCoroutine(ComprobarSiMueve(FindObjectsOfType<ActionableObject>()));
        }else{
            StartCoroutine(SubirBellota(verde));
        }            
        animator_ardilla.SetTrigger("saltar");
    }
    private IEnumerator SubirBellota(Transform bellota){
        float velocidad = 0.15f;
        Vector3 destino = new Vector3(bellota.position.x, bellota.position.y + 0.15f, bellota.position.z);
        while (Vector3.Distance(bellota.position, destino) > 0.001f)
        {
            bellota.position = Vector3.MoveTowards(bellota.position, destino, velocidad * Time.deltaTime);
            yield return null;
        }
        FloatEffect floatEffect = bellota.gameObject.AddComponent<FloatEffect>();
        floatEffect.floatSpeed = 1f;
        floatEffect.floatHeight = 0.03f;
    }

    private IEnumerator ComprobarSiMueve(ActionableObject[] actionableObjects){
        foreach (ActionableObject actionableObject in actionableObjects){
            while(actionableObject.GetProgramableObject().GetZonaProgramacion().GetEstaEjecutando()){
                if(actionableObject.HasMoved()){
                    nivel.Lose("El semaforo esta en rojo hemos enfadado a la ardilla");
                    yield break;
                }
                yield return null;
            }
        }
        nivel.Win();
    }

    private void ResetearBellota(Transform bellota){
        FloatEffect floatEffect = bellota.GetComponent<FloatEffect>();
        if (floatEffect != null)
        {
            Destroy(floatEffect);
        }        
        bellota.localPosition = new Vector3(bellota.localPosition.x, 0.01f, bellota.localPosition.z);
    }
}
