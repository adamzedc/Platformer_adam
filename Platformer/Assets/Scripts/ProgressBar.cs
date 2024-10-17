using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private Slider slider;
    public float barRemoveSpeed = 1f;
    public Weapon weapon;


    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }
    // Update is called once per frame
    void Update()
    {
        //If the bullet hits a target
        if (weapon.hit)
        {
            OnTargetHit();
        }
        slider.value -= barRemoveSpeed * Time.deltaTime;
    }

    public void OnTargetHit() {
        //Reset the players health
        slider.value = slider.maxValue;
        weapon.hit = false;
    }
}
