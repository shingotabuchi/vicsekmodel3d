using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraMovement : MonoBehaviour {

    public float lookSpeedH = 5f;
    public float lookSpeedV = 5f;
    public float zoomSpeed = 20f;
    public float dragSpeed = 70f;

    private float yaw = 0f;
    private float pitch = 0f;

    public float xpluslim,xminuslim,ypluslim,yminuslim,zpluslim,zminuslim;
    void Start(){
        pitch = transform.eulerAngles.x;
        yaw = transform.eulerAngles.y;
    }
    void Update ()
    {
        //Look around with Right Mouse
        if (Input.GetMouseButton(1))
        {
            yaw += lookSpeedH * Input.GetAxis("Mouse X");
            pitch -= lookSpeedV * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        }
        Vector3 newposition;
        //drag camera around with Middle Mouse
        if (Input.GetMouseButton(2))
        {
            float xTrans = -Input.GetAxisRaw("Mouse X") * Time.deltaTime * dragSpeed;
            float yTrans = -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * dragSpeed;
            transform.Translate(xTrans, yTrans, 0);
            if(!CheckPosition(transform.position)){
                transform.Translate(-xTrans, -yTrans, 0);
            }
        }

        //Zoom in and out with Mouse Wheel
        float zoom = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        transform.Translate(0, 0, zoom, Space.Self);
        if(!CheckPosition(transform.position)){
            transform.Translate(0, 0, -zoom, Space.Self);
        }
    }

    bool CheckPosition(Vector3 newposition){
        // if(newposition.x>xpluslim) return false; 
        // if(newposition.x<xminuslim) return false; 
        // if(newposition.y>ypluslim) return false; 
        // if(newposition.y<yminuslim) return false; 
        // if(newposition.z>zpluslim) return false; 
        // if(newposition.z<zminuslim) return false; 
        return true;
    }
    
}