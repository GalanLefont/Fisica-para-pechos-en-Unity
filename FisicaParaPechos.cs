using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FisicaParaPechos : MonoBehaviour
{
    public Transform contenedor;

    Transform puntoDeReferencia;
    Vector3 posicionRelativaInicial;
    public Vector3 offSetPosicionInicial;

    float distanciaABaseInicial;
    public Transform puntoBase;    

    Vector3 velocidad;

    public float masa = 1;
    [Range(0, 1000)]
    public float constanteElastica = 100;
    [Range(0, 1000)]
    public float constanteElasticaCompresion = 100;

    public float distanciaMaxima;

    [Range(0,10)]
    public float escalaDeGravead = 1;

    [Range(0,0.2f)]
    public float amortiguado;

    void Start()
    {
        puntoDeReferencia = transform.parent;
        posicionRelativaInicial = puntoDeReferencia.InverseTransformPoint(transform.position);

        distanciaABaseInicial = (puntoBase.position - transform.position).magnitude;

        transform.parent = contenedor;
    }

   
    private void FixedUpdate()
    {
        transform.rotation = puntoBase.rotation;

        //Limitar distancia
        if((puntoDeReferencia.TransformPoint(posicionRelativaInicial) - transform.position).magnitude > distanciaMaxima)
        {              
            transform.position = puntoDeReferencia.TransformPoint(posicionRelativaInicial) +
                (transform.position - puntoDeReferencia.TransformPoint(posicionRelativaInicial)).normalized * distanciaMaxima;

            velocidad = Vector3.zero;
                //velocidad = [rigidbody].velocity;
        }


        Vector3 fuerzaNeta = Vector3.zero;

        //Fuerza Elastica
        fuerzaNeta += constanteElastica * (puntoDeReferencia.TransformPoint
            (posicionRelativaInicial + offSetPosicionInicial) - transform.position);
        
        fuerzaNeta += ((puntoBase.position - transform.position).magnitude - distanciaABaseInicial)
            * constanteElasticaCompresion * (puntoBase.position - transform.position).normalized;

        //Fuerza De Gravedad
        fuerzaNeta += Physics.gravity * masa * escalaDeGravead;

        //Amortiguado
        velocidad -= velocidad * amortiguado;
            //velocidad += ([rigidbody].velocity - velocidad) * amortiguado

        //Aplicar las fuerzas
        velocidad += fuerzaNeta / masa * Time.deltaTime;
        transform.position += velocidad * Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(
            Application.isPlaying ? puntoDeReferencia.TransformPoint(posicionRelativaInicial) :
            transform.position
            , distanciaMaxima);
    }
}
