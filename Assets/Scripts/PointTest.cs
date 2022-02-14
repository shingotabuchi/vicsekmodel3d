using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTest : MonoBehaviour
{
    public GameObject point;
    public int samples;
    public float fov;
    // Start is called before the first frame update
    void Start()
    {
    }
    IEnumerator plotshit(){
        float phi = Mathf.PI * (3f - Mathf.Sqrt(5f));
        for (int i = 0; i < samples; i++)
        {
            Vector3 spawnpoint = new Vector3(0,0,0);
            spawnpoint.y = 1f - ((float)i)/(samples - 1f) * (1-Mathf.Cos(Mathf.PI*fov/180f));
            float radius = Mathf.Sqrt(1 - spawnpoint.y * spawnpoint.y);
            float theta = phi * i;
            spawnpoint.x = Mathf.Cos(theta) * radius;
            spawnpoint.z = Mathf.Sin(theta) * radius;
            Instantiate(point,spawnpoint,Quaternion.identity);
            Debug.DrawRay(new Vector3(0,0,0), spawnpoint, Color.red);
            yield return new WaitForSeconds(0.05f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("a")){
            StartCoroutine(plotshit());
        }
    }
}
