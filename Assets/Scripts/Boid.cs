using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public static int frameInterval;
    public static float timeInterval;
    public static float worldSize;
    public static float viewRadius;
    public static float zor,zoo,zoa;
    public static float turnRate;
    public static float fov;
    public static float noiseSigma;
    public static bool Repulse,Orient,Attract,Noise;
    public static int pathSearchResolution;
    public static bool isShuukiKyoukai;
    public static GameObject mirrorPrefab;
    bool hasActivatedMirrors;
    float turnRateRadians;
    public static float velocity;
    bool noObstacles = true;
    Vector3 desiredDirection = new Vector3(0,0,0);
    public static Transform mirrorBoidParent;
    Vector3[] unitMirrorOffsets = new Vector3[6]{
        new Vector3(1,0,0),
        new Vector3(-1,0,0),
        new Vector3(0,1,0),
        new Vector3(0,-1,0),
        new Vector3(0,0,1),
        new Vector3(0,0,-1)
    };
    GameObject[] mirrorBoids = new GameObject[6];
    // Start is called before the first frame update
    void Start()
    {
        turnRateRadians = Mathf.PI*turnRate/180f;
        StartCoroutine(BoidBehavior());
        CreateMirrorImage();
        // if(isShuukiKyoukai){
        //     CreateMirrorImage();
        //     hasCreatedMirrors = true;
        // }
        // else hasCreatedMirrors = false;
    }
    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.up*viewRadius, Color.red);
        Move();
        CheckForObstacles();
        CheckBoundaries();
        if(isShuukiKyoukai)CheckForWalls();
        else if(hasActivatedMirrors){
            for (int i = 0; i < 6; i++)
            {
                mirrorBoids[i].SetActive(false);
                hasActivatedMirrors = false;
            }
        }
    }
    void OnDestroy() {
        for (int i = 0; i < 6; i++)
        {
            Destroy(mirrorBoids[i]);
        }
    }
    void CheckForWalls(){
        for (int i = 0; i < 6; i++)
        {
            if(Physics.Raycast(transform.position, -unitMirrorOffsets[i], zoa, 1 << LayerMask.NameToLayer("Wall"))){
                mirrorBoids[i].SetActive(true);
                hasActivatedMirrors = true;
            }
            else mirrorBoids[i].SetActive(false);
        }
    }
    void CreateMirrorImage(){
        for (int i = 0; i < 6; i++)
        {
            mirrorBoids[i] = Instantiate(mirrorPrefab,transform.position + 2*worldSize*unitMirrorOffsets[i],transform.rotation);
            mirrorBoids[i].transform.parent = mirrorBoidParent;
            MirrorBoid mirrorScript = mirrorBoids[i].GetComponent<MirrorBoid>();
            mirrorScript.offset = 2*worldSize*unitMirrorOffsets[i];
            mirrorScript.parent = transform;
            mirrorBoids[i].SetActive(false);
        }
    }
    void Move(){
        transform.up = Vector3.RotateTowards(transform.up, desiredDirection, turnRateRadians*Time.deltaTime, 0.0f);
        transform.position += velocity*Time.deltaTime*transform.up;
    }
    void CheckForObstacles(){
        if(Physics.Raycast(transform.position, transform.up, viewRadius, 1 << LayerMask.NameToLayer("Obstacle"))){
            noObstacles = false;
            float phi = Mathf.PI * (3f - Mathf.Sqrt(5f));
            for (int i = 0; i < pathSearchResolution; i++)
            {
                Vector3 pathFindCoefficient = new Vector3(0,0,0);
                Vector3 upPerpendicular1 = new Vector3(0,0,0);
                Vector3 upPerpendicular2 = new Vector3(0,0,0);
                for (int j = 0; j < 3; j++)
                {
                    if(transform.up[j]!=0){
                        upPerpendicular1[j] = -transform.up[(j+1)%3]/transform.up[j];
                        upPerpendicular1[(j+1)%3] = 1;
                        upPerpendicular1[(j+2)%3] = 0;
                        
                        for (int k = 0; k < 3; k++)
                        {
                            upPerpendicular2[k] = transform.up[(k+1)%3]*upPerpendicular1[(k+2)%3] - transform.up[(k+2)%3]*upPerpendicular1[(k+1)%3];
                        }
                        upPerpendicular1.Normalize();
                        upPerpendicular2.Normalize();
                        break;
                    }
                }
                pathFindCoefficient.y = 1f - (((float)i)/(pathSearchResolution - 1f)) * (1-Mathf.Cos(Mathf.PI*fov/180f));
                float radius = Mathf.Sqrt(1 - pathFindCoefficient.y * pathFindCoefficient.y);
                float theta = phi * i;
                pathFindCoefficient.x = Mathf.Cos(theta) * radius;
                pathFindCoefficient.z = Mathf.Sin(theta) * radius;

                Vector3 pathFindRay = transform.up * pathFindCoefficient.y + upPerpendicular1 * pathFindCoefficient.x + upPerpendicular2 * pathFindCoefficient.z;
                Debug.DrawRay(transform.position, pathFindRay*viewRadius, Color.red,0.1f);
                if(!Physics.Raycast(transform.position, pathFindRay, viewRadius, 1 << LayerMask.NameToLayer("Obstacle")) || i == pathSearchResolution-1){
                    desiredDirection = pathFindRay.normalized;
                    break;
                }
            }
        }
        else{
            noObstacles = true;
        }
    }
    IEnumerator BoidBehavior(){
        while(true){
            if(noObstacles){
                Collider[] hitColliders = Physics.OverlapSphere(transform.position,zoa,1 << LayerMask.NameToLayer("Boid"));
                Vector3 orientDirection = new Vector3(0,0,0);
                Vector3 attractDirection = new Vector3(0,0,0);
                Vector3 repulseDirection = new Vector3(0,0,0);
                int nearbyBoidsCount = hitColliders.Length-1;
                int boidsInZorCount = 0;
                int boidsInZooCount = 0;
                int boidsInZoaCount = 0;
                bool repulsiveMode = false;
                foreach (var collider in hitColliders)
                {
                    if(collider.transform.gameObject==null){
                        nearbyBoidsCount -= 1;
                        continue;
                    }
                    if(collider.transform.gameObject==gameObject){
                        continue;
                    }
                    if(Vector3.Angle(transform.up,collider.transform.position - transform.position)>fov){
                        nearbyBoidsCount -= 1;
                        continue;
                    }
                    float dist = GetDistance(transform.position,collider.transform.position);
                    if(dist<zor){
                        if(Repulse) repulsiveMode = true;
                        repulseDirection -= collider.transform.position - transform.position;
                        boidsInZorCount += 1;
                    }
                    else if(repulsiveMode==false){
                        if(dist<zoo){
                            orientDirection += collider.transform.up;
                            boidsInZooCount += 1;
                        }
                        else{
                            attractDirection += collider.transform.position - transform.position;
                            boidsInZoaCount += 1;
                        }
                    }
                }
                if(nearbyBoidsCount>0){
                    if(repulsiveMode){
                        desiredDirection = repulseDirection.normalized;
                    }
                    else{
                        if(boidsInZooCount>0){
                            if(Orient) desiredDirection = orientDirection.normalized;
                            if(boidsInZoaCount>0){
                                if(Orient&&Attract){
                                    
                                    // desiredDirection += attractDirection.normalized;
                                    desiredDirection = orientDirection.normalized*boidsInZooCount + attractDirection.normalized*boidsInZoaCount;
                                    desiredDirection.Normalize();
                                }
                                else if(Attract) desiredDirection = attractDirection.normalized;
                            }
                        }
                        else if(boidsInZoaCount>0){
                            if(Attract) desiredDirection = attractDirection.normalized;
                        }
                    }

                    if(desiredDirection.magnitude == 0){
                        desiredDirection = transform.up;
                    }
                }
                else{
                    desiredDirection = transform.up;
                }

                if(Noise){
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
                }
                yield return new WaitForSeconds(timeInterval);
            }
            yield return null;
            // if(frameInterval>0)yield return StartCoroutine(WaitFor.Frames(frameInterval));
            // else yield return null;
        }
    }
    float GetDistance(Vector3 a, Vector3 b){
        return Mathf.Sqrt( (a.x-b.x)*(a.x-b.x) + (a.y-b.y)*(a.y-b.y) + (a.z-b.z)*(a.z-b.z) );
    }
    void CheckBoundaries(){
        if(isShuukiKyoukai){
            if(Mathf.Abs(transform.position.x)>worldSize){
                if(transform.position.x>0)transform.position = new Vector3(-worldSize,transform.position.y,transform.position.z);
                else transform.position = new Vector3(worldSize,transform.position.y,transform.position.z);
            }
            if(Mathf.Abs(transform.position.y-worldSize)>worldSize){
                if(transform.position.y-worldSize>0)transform.position = new Vector3(transform.position.x,0,transform.position.z);
                else transform.position = new Vector3(transform.position.x,worldSize*2,transform.position.z);
            }
            if(Mathf.Abs(transform.position.z)>worldSize){
                if(transform.position.z>0)transform.position = new Vector3(transform.position.x,transform.position.y,-worldSize);
                else transform.position = new Vector3(transform.position.x,transform.position.y,worldSize);
            }
        }
        else{
            if(Mathf.Abs((transform.position + 0.2f*transform.up).x)>worldSize){
                if((transform.position + 0.2f*transform.up).x>0)transform.position = new Vector3(worldSize,transform.position.y,transform.position.z) - 0.2f*transform.up;
                else transform.position = new Vector3(-worldSize,transform.position.y,transform.position.z) - 0.2f*transform.up;
            }
            if(Mathf.Abs((transform.position + 0.2f*transform.up).y-worldSize)>worldSize){
                if((transform.position + 0.2f*transform.up).y-worldSize>0)transform.position = new Vector3(transform.position.x,worldSize*2,transform.position.z) - 0.2f*transform.up;
                else transform.position = new Vector3(transform.position.x,0,transform.position.z) - 0.2f*transform.up;
            }
            if(Mathf.Abs((transform.position + 0.2f*transform.up).z)>worldSize){
                if((transform.position + 0.2f*transform.up).z>0)transform.position = new Vector3(transform.position.x,transform.position.y,worldSize) - 0.2f*transform.up;
                else transform.position = new Vector3(transform.position.x,transform.position.y,-worldSize) - 0.2f*transform.up;
            }
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
