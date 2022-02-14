using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZooSlider : MonoBehaviour
{
    public Slider ZoaSlider;
    public Slider maxLimitSlider;
    public Slider minLimitSlider;
    public RectTransform uiCircle;
    public Text zooText;
    Vector3 allOne = new Vector3(1,1,1);
    Slider thisSlider;
    public bool isZoo = true;
    void Awake(){
        thisSlider = GetComponent<Slider>();
    }
    void Update()
    {
        if(thisSlider.maxValue != maxLimitSlider.value){
            thisSlider.maxValue = maxLimitSlider.value;
        }
        if(minLimitSlider!=null && thisSlider.minValue != minLimitSlider.value){
            thisSlider.minValue = minLimitSlider.value;
        }
        if(thisSlider.value != ZoaSlider.value * uiCircle.sizeDelta.y*uiCircle.localScale.x/1000f){
            thisSlider.value = ZoaSlider.value * uiCircle.sizeDelta.y*uiCircle.localScale.x/1000f;
            zooText.text = thisSlider.value.ToString();
            if(isZoo)Boid.zoo = thisSlider.value;
            else Boid.zor = thisSlider.value;
        }
    }
    public void OnChange(){
        zooText.text = thisSlider.value.ToString();
        if(isZoo)Boid.zoo = thisSlider.value;
        else Boid.zor = thisSlider.value;
        uiCircle.localScale = allOne*thisSlider.value*1000f/(ZoaSlider.value*uiCircle.sizeDelta.y);
    }
}
