using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProgressBar : MonoBehaviour
{
    private Slider slider;
    public float barRemoveSpeed = 1f;
    private bool _isRunning;

    private void OnEnable()
    {
        BarEventManager.SliderReset += BarEventManager_SliderReset;
    }

    private void OnDisable()
    { 
        BarEventManager.SliderReset -= BarEventManager_SliderReset;
    }

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }
    void Update()
    {
        //Reduce the players health each frame
        slider.value -= barRemoveSpeed * Time.deltaTime;
        if(slider.value <= 0)
        {
            //Player is dead
            SceneManager.LoadScene("Menu");
            Debug.Log("Player is dead");
        }
    }


    //Reset the players health
    private void BarEventManager_SliderReset() => slider.value = slider.maxValue;

}
