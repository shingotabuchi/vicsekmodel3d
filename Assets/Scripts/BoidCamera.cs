using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoidCamera : MonoBehaviour
{
    public Transform BoidParent; 
    public GameObject mainCameraObject;
    Camera mainCamera;
    bool isEnabled = false;
    int followingBoidIndex = 0;
    Vector3 desiredPosition,desiredMuki;
    public float initBoidBackwardDistance = 1f;
    float boidBackwardDistance;
    public float boidUpwardDistance = 0.5f;
    public float speed,turnRate;
    Camera thisCamera;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    private Vector3 velocityMovement = Vector3.zero;
    public float zoomSpeed;
    public float haba;
    public GameObject leftRightText;
    Coroutine lastCoroutine;
    void Start(){
        thisCamera = GetComponent<Camera>();
        mainCamera = mainCameraObject.GetComponent<Camera>();
    }
    void Update()
    {
        if(Input.GetKeyDown("b")&&BoidParent.childCount!=0){
            mainCamera.enabled = isEnabled;
            thisCamera.enabled = !isEnabled;
            isEnabled = !isEnabled;
            if(isEnabled){
                lastCoroutine = StartCoroutine(FollowBoid());
                leftRightText.SetActive(true);
            }
            else{
                StopCoroutine(lastCoroutine);
                transform.position = mainCameraObject.transform.position;
                transform.rotation = mainCameraObject.transform.rotation;
                leftRightText.SetActive(false);
            }
        }
    }
    IEnumerator FollowBoid(){
        while(true){
            if(BoidParent.childCount==0){
                mainCamera.enabled = isEnabled;
                thisCamera.enabled = !isEnabled;
                isEnabled = !isEnabled;
                StopCoroutine(lastCoroutine);
                transform.position = mainCameraObject.transform.position;
                transform.rotation = mainCameraObject.transform.rotation;
                leftRightText.SetActive(false);
                yield return null;
            }
            if(Input.GetKeyDown("left")){
                followingBoidIndex = (followingBoidIndex-1)%BoidParent.childCount;
            }
            if(Input.GetKeyDown("right")){
                followingBoidIndex = (followingBoidIndex+1)%BoidParent.childCount;
            }
            float zoom = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            initBoidBackwardDistance -= zoom;
            Vector3 forwardVector = BoidParent.GetChild(followingBoidIndex).up;
            Vector3 upVector = Boid.RotateAroundAxis(forwardVector,CrossProduct(forwardVector,new Vector3(0,1,0)),90);
            boidBackwardDistance = initBoidBackwardDistance - smoothTime*Boid.velocity;
            desiredPosition = BoidParent.GetChild(followingBoidIndex).position-boidBackwardDistance * forwardVector + boidUpwardDistance * upVector;
            desiredMuki = forwardVector;
            transform.forward = Vector3.SmoothDamp(transform.forward, desiredMuki, ref velocity, smoothTime);
            transform.position = Vector3.SmoothDamp(transform.position,desiredPosition,  ref velocityMovement, smoothTime);
            CheckBoundaries();
            yield return null;
        }
    }
    void CheckBoundaries(){
        if(Mathf.Abs(transform.position.x)>(Boid.worldSize-haba)){
            if(transform.position.x>0)transform.position = new Vector3((Boid.worldSize-haba),transform.position.y,transform.position.z);
            else transform.position = new Vector3(-(Boid.worldSize-haba),transform.position.y,transform.position.z);
        }
        if(Mathf.Abs(transform.position.y-(Boid.worldSize-haba))>(Boid.worldSize-haba)){
            if(transform.position.y-(Boid.worldSize-haba)>0)transform.position = new Vector3(transform.position.x,(Boid.worldSize-haba)*2,transform.position.z);
            else transform.position = new Vector3(transform.position.x,0,transform.position.z);
        }
        if(Mathf.Abs(transform.position.z)>(Boid.worldSize-haba)){
            if(transform.position.z>0)transform.position = new Vector3(transform.position.x,transform.position.y,(Boid.worldSize-haba));
            else transform.position = new Vector3(transform.position.x,transform.position.y,-(Boid.worldSize-haba));
        }
    }
    Vector3 CrossProduct(Vector3 a, Vector3 b){
        return new Vector3(
            a.y*b.z - a.z*b.y,
            a.z*b.x - a.x*b.z,
            a.x*b.y - a.y*b.x
        );
    }
}
