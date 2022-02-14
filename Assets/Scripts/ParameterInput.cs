using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParameterInput : MonoBehaviour
{
    // Text thisText;
    public InputField thisField;
    public Text PlaceHolder;
    public Slider slider;
    public bool isZoa = false;
    public bool isZoo = false;
    public bool isZor = false;
    void Start()
    {
        // thisText = GetComponent<Text>();
    }
    public void OnChange(){
        float textFloat;
        bool success = float.TryParse( thisField.text ,out textFloat );
        if(success){
            if(slider.minValue<=textFloat&&textFloat<=slider.maxValue){
                thisField.text = "";
                if(!isZoa){
                    ZooSlider sliderScript = slider.transform.gameObject.GetComponent<ZooSlider>();
                    slider.value = textFloat;
                    if(isZoo){
                        Boid.zoo = textFloat;
                    }
                    else Boid.zor = textFloat;
                    sliderScript.OnChange();
                }
                else{
                    Sliders sliderScript = slider.transform.gameObject.GetComponent<Sliders>();
                    slider.value = textFloat;
                    sliderScript.ChangeZoa();
                }
            }
            else{
                thisField.text = "";
            }
        }
        else{
            thisField.text = "";
        }
    }
}
