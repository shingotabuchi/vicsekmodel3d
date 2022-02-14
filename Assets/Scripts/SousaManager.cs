using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  

public class SousaManager : MonoBehaviour
{
    public GameObject Sousa,Theory;
    void Start()
    {
        Sousa.SetActive(MenuManager.isSousa);
        Theory.SetActive(!MenuManager.isSousa);
    }
    public void BackToMenu(){
        SceneManager.LoadScene("Menu");
    }
}
