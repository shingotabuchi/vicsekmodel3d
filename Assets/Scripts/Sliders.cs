using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sliders : MonoBehaviour
{
    public Slider zoaSlider;
    public Text zoaText;
    public RectTransform boidIcon;
    public void ChangeZoa(){
        Vector3 allOne = new Vector3(1,1,1);
        boidIcon.localScale =  allOne*(500f*0.41f/(258.9391f*zoaSlider.value));
        zoaText.text = zoaSlider.value.ToString();
        Boid.zoa = zoaSlider.value;
    }
    void Update(){
        if(zoaSlider.value!=500f*0.41f/(258.9391f*boidIcon.localScale.x)){
            zoaSlider.value = 500f*0.41f/(258.9391f*boidIcon.localScale.x);
            ChangeZoa();
        }
    }
}
