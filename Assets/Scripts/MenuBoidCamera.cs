using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBoidCamera : MonoBehaviour
{
    Vector3 desiredPosition,desiredMuki;
    public float initBoidBackwardDistance = 1f;
    float boidBackwardDistance;
    public float boidUpwardDistance = 0.5f;
    public float smoothTime = 0.3F;
    Vector3 velocity = Vector3.zero;
    Vector3 velocityMovement = Vector3.zero;
    public Transform boidtransform;
    // void Start(){
    //     StartCoroutine(FollowBoid());
    // }
    // IEnumerator FollowBoid(){
    //     while(true){
    //         Vector3 forwardVector = boidtransform.up;
    //         Vector3 upVector = Boid.RotateAroundAxis(forwardVector,CrossProduct(forwardVector,new Vector3(0,1,0)),90);
    //         boidBackwardDistance = initBoidBackwardDistance - smoothTime*Boid.velocity;
    //         desiredPosition = boidtransform.position-boidBackwardDistance * forwardVector + boidUpwardDistance * upVector;
    //         desiredMuki = forwardVector;
    //         transform.forward = Vector3.SmoothDamp(transform.forward, desiredMuki, ref velocity, smoothTime);
    //         transform.position = Vector3.SmoothDamp(transform.position,desiredPosition,  ref velocityMovement, smoothTime);
    //         yield return null;
    //     }
    // }
    void Update(){
        Vector3 forwardVector = boidtransform.up;
        Vector3 upVector = Boid.RotateAroundAxis(forwardVector,CrossProduct(forwardVector,new Vector3(0,1,0)),90);
        boidBackwardDistance = initBoidBackwardDistance - smoothTime*7;
        desiredPosition = boidtransform.position-boidBackwardDistance * forwardVector + boidUpwardDistance * upVector;
        desiredMuki = forwardVector;
        transform.forward = Vector3.SmoothDamp(transform.forward, desiredMuki, ref velocity, smoothTime);
        transform.position = Vector3.SmoothDamp(transform.position,desiredPosition,  ref velocityMovement, smoothTime);
    }
    Vector3 CrossProduct(Vector3 a, Vector3 b){
        return new Vector3(
            a.y*b.z - a.z*b.y,
            a.z*b.x - a.x*b.z,
            a.x*b.y - a.y*b.x
        );
    }
}
