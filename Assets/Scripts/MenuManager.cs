using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  

public class MenuManager : MonoBehaviour
{
    public static bool isSousa;
    public MenuBoid boid;
    public void GotoHaikyo(){
        SceneManager.LoadScene("BigBoxTest");
        boid.StopThatShit();
    }
    public void GotoDekaiHako(){
        SceneManager.LoadScene("OpenBigBox");
        boid.StopThatShit();
    }
    public void Hako(){
        SceneManager.LoadScene("SmallBoxTest");
        boid.StopThatShit();
    }
    public void Hako2(){
        SceneManager.LoadScene("ObstacleCourse");
        boid.StopThatShit();
    }
    public void Sousa(){
        isSousa = true;
        SceneManager.LoadScene("Sousa");
        boid.StopThatShit();
    }
    public void Theory(){
        isSousa = false;
        SceneManager.LoadScene("Sousa");
        boid.StopThatShit();
    }
    public void Quit(){
        Application.Quit();
    }
    
}
