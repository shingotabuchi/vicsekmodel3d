using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//speed slider
public class ParameterInputV2 : MonoBehaviour
{
    public Slider slider;
    public InputField field;
    public BoidManager boidManager;
    public void ChangeField(){
        float textFloat;
        bool success = float.TryParse( field.text ,out textFloat );
        if(success){
            if(slider.minValue<=textFloat&&textFloat<=slider.maxValue){
                slider.value = textFloat;
                Boid.velocity = textFloat;
            }
            else{
                field.text = slider.value.ToString();
            }
        }
        else{
            field.text = slider.value.ToString();
        }
    }
    public void ChangeSlider(){
        field.text = slider.value.ToString();
        Boid.velocity = slider.value;
    }
    public void ChangeCountField(){
        int textInt;
        bool success = int.TryParse( field.text ,out textInt );
        if(success){
            if(slider.minValue<=textInt&&textInt<=slider.maxValue){
                slider.value = textInt;
                int initBoidCount = boidManager.boidCount;
                if(boidManager.boidCount>textInt){
                    for(int i = 0; i < initBoidCount-textInt; i++){
                        boidManager.boidCount--;
                        Destroy(boidManager.boidParent.GetChild(0).gameObject);
                    }
                }
                else if(boidManager.boidCount<textInt){
                    for(int i = 0; i < textInt-initBoidCount; i++){
                        boidManager.boidCount++;
                        boidManager.SpawnBoid();
                    }
                }
            }
            else{
                field.text = slider.value.ToString();
            }
        }
        else{
            field.text = slider.value.ToString();
        }
    }
    public void ChangeCountSlider(){
        field.text = slider.value.ToString();
        int initBoidCount = boidManager.boidCount;
        if(boidManager.boidCount>slider.value){
            for(int i = 0; i < initBoidCount-slider.value; i++){
                boidManager.boidCount--;
                Destroy(boidManager.boidParent.GetChild(i).gameObject);
            }
        }
        else if(boidManager.boidCount<slider.value){
            for(int i = 0; i < slider.value-initBoidCount; i++){
                boidManager.boidCount++;
                boidManager.SpawnBoid();
            }
        }
    }
}
