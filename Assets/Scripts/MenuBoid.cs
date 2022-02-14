using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBoid : MonoBehaviour
{
    public float timeInterval;
    public float turnRate;
    public float fov;
    public float noiseSigma;
    float turnRateRadians;
    public float velocity;
    Vector3 desiredDirection = new Vector3(0,0,0);
    public Camera boidCamera;
    Coroutine poop;
    void Start()
    {
        turnRateRadians = Mathf.PI*turnRate/180f;
        poop = StartCoroutine(BoidBehavior());
    }
    public void StopThatShit(){
        StopCoroutine(poop);
    }
    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void Move(){
        transform.up = Vector3.RotateTowards(transform.up, desiredDirection, turnRateRadians*Time.deltaTime, 0.0f);
        transform.position += velocity*Time.deltaTime*transform.up;
    }
    IEnumerator BoidBehavior(){
        while(true){
            // desiredDirection = transform.up;
            desiredDirection = boidCamera.ScreenPointToRay(Input.mousePosition).direction;
            Vector3 desiredDirectionPerpendicular = new Vector3(0,0,0);
            for (int i = 0; i < 3; i++)
            {
                if(desiredDirection[i]!=0){
                    desiredDirectionPerpendicular[i] = -desiredDirection[(i+1)%3]/desiredDirection[i];
                    desiredDirectionPerpendicular[(i+1)%3] = 1;
                    desiredDirectionPerpendicular[(i+2)%3] = 0;
                    break;
                }
            }
            float rotateAngle = Mathf.Abs(GaussianRand(noiseSigma));
            if(rotateAngle>180f)rotateAngle = 180f;
            Vector3 newDesiredDirection = RotateAroundAxis(desiredDirection,desiredDirectionPerpendicular,rotateAngle);
            desiredDirection = RotateAroundAxis(newDesiredDirection,desiredDirection,Random.Range(0f,360f));
            yield return new WaitForSeconds(timeInterval);
        }
    }
    public static Vector3 RotateAroundAxis(Vector3 vectorToRotate, Vector3 axis, float rotationDegrees){
        float c,s;
        c = Mathf.Cos(Mathf.PI*rotationDegrees/180f);
        s = Mathf.Sin(Mathf.PI*rotationDegrees/180f);
        axis.Normalize();
        float[,] rotationMatrix = new float[3,3]{
            { c + axis.x*axis.x*(1-c),          axis.x*axis.y*(1-c) - axis.z*s, axis.x*axis.z*(1-c) + axis.y*s},
            { axis.y*axis.x*(1-c) + axis.z*s,   c + axis.y*axis.y*(1-c),        axis.y*axis.z*(1-c) - axis.x*s},
            { axis.z*axis.x*(1-c) - axis.y*s,   axis.z*axis.y*(1-c) + axis.x*s, c + axis.z*axis.z*(1-c)       }
        };
        Vector3 returnVector = new Vector3(0,0,0);
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                returnVector[i] += vectorToRotate[j]*rotationMatrix[i,j];
            }
        }
        return returnVector;
    }
    float GaussianRand(float sigma){
        return Mathf.Sqrt(-2*sigma*Mathf.Log(Random.Range(0f,1f))) * Mathf.Cos(2*Mathf.PI*Random.Range(0f,1f));
    }
}
