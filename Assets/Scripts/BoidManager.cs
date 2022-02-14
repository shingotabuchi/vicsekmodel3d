using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoidManager : MonoBehaviour
{
    public int boidCount;
    public GameObject boidPrefab;
    public Transform boidParent; 
    public float maxVelocity,minVelocity;
    public float worldSize;
    public int frameInterval;
    public float viewRadius;
    public float zor,zoo,zoa;
    public float turnRate;
    public float timeInterval;
    public float fov;
    public float noiseSigma;
    public int pathSearchResolution;
    public bool Repulse,Orient,Attract,Noise;
    public InputField NoiseSigmaField, SpeedField, FovField, ZoaField, ZooField, Zorfield;
    public ParameterInputV2 SpeedSlider;
    public FovKnob fovKnob;
    public ParameterInput ZoaInput, ZooInput, ZorInput;
    public GameObject settings;
    bool EvasionIsOn = true;
    public GameObject[] Walls = new GameObject[6];
    public GameObject mirrorPrefab;
    public Transform mirrorBoidParent;
    // Start is called before the first frame update
    void Start()
    {
        Boid.worldSize = worldSize;
        Boid.frameInterval = frameInterval;
        Boid.viewRadius = viewRadius;
        Boid.zor = zor;
        Boid.zoa = zoa;
        Boid.zoo = zoo;
        Boid.turnRate = turnRate;
        Boid.timeInterval = timeInterval;
        Boid.fov = fov;
        Boid.noiseSigma = noiseSigma;
        Boid.Repulse = Repulse;
        Boid.Orient = Orient;
        Boid.Attract = Attract;
        Boid.Noise = Noise;
        Boid.pathSearchResolution = pathSearchResolution;
        Boid.velocity = maxVelocity;
        Boid.isShuukiKyoukai = !EvasionIsOn;
        Boid.mirrorPrefab = mirrorPrefab;
        Boid.mirrorBoidParent = mirrorBoidParent;

        NoiseSigmaField.text = noiseSigma.ToString();
        SpeedField.text = maxVelocity.ToString();
        SpeedSlider.ChangeField();
        FovField.text = fov.ToString();
        fovKnob.ChangeField();
        ZoaField.text = zoa.ToString();
        ZoaInput.OnChange();
        ZooField.text = zoo.ToString();
        ZooInput.OnChange();
        Zorfield.text = zor.ToString();
        ZorInput.OnChange();
        settings.SetActive(false);

        for (int i = 0; i < boidCount; i++)SpawnBoid();
    }
    public void SpawnBoid(){
        Vector3 spawnPoint = new Vector3(Random.Range(-worldSize,worldSize), Random.Range(-worldSize,worldSize), Random.Range(-worldSize,worldSize));
        spawnPoint += new Vector3(0,worldSize,0);
        Instantiate(boidPrefab,spawnPoint,Quaternion.Euler(Random.Range(0f,360f),Random.Range(0f,360f),Random.Range(0f,360f)),boidParent);
    }

    IEnumerator CheckParameters(){
        while(true){
            print("data");
            print(Boid.noiseSigma);
            print(Boid.velocity);
            print(Boid.fov);
            print(Boid.zoa);
            print(Boid.zoo);
            print(Boid.zor);
            yield return new WaitForSeconds(2f);
        }
    }
    public void UpdateSigmaFromInputField(){
        float textFloat;
        bool success = float.TryParse( NoiseSigmaField.text ,out textFloat );
        if(success){
            if(textFloat>0)Boid.noiseSigma = textFloat;
            else NoiseSigmaField.text = Boid.noiseSigma.ToString();
        }
        else{
            NoiseSigmaField.text = Boid.noiseSigma.ToString();
        }

        // success = float.TryParse( SpeedField.text ,out textFloat );
        // if(success){
        //     if(0.1<=textFloat&&textFloat<=15){
        //         Boid.velocity = textFloat;
        //     }
        //     else{
        //         SpeedField.text = Boid.velocity.ToString();
        //     }
        // }
        // else{
        //     SpeedField.text = Boid.velocity.ToString();
        // }
    }
    public void ToggleEvasion(){
        if(EvasionIsOn){
            for (int i = 0; i < 6; i++)
            {
                Walls[i].layer = LayerMask.NameToLayer("Wall");
            }
            EvasionIsOn = false;
            Boid.isShuukiKyoukai = true;
        }
        else{
            for (int i = 0; i < 6; i++)
            {
                Walls[i].layer = LayerMask.NameToLayer("Obstacle");
            }
            EvasionIsOn = true;
            Boid.isShuukiKyoukai = false;
        }
    }
}
