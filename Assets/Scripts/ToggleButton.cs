using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : MonoBehaviour
{
    public GameObject Canvas;
    bool on = false;
    public void Toggle(){
        on = !on;
        Canvas.SetActive(on);
    }
}
