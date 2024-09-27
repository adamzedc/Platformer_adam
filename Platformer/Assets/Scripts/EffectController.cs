using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{

    public Rigidbody playerRB;
    public ParticleSystem ps;

    [Header("Values")]
    public float rateOverTime;
    public float effectSpeed;

    [Header("Number of particles")]
    private float emissionMultiplier;
    public float minRateMultiplier;
    public float maxRateMultiplier;


    [Header("Speed of particles")]
    private float speedMultiplier;
    public float minSpeedMultiplier;
    public float maxSpeedMultiplier;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var emission = ps.emission;
        var main = ps.main;

        changeSpeedMultiplier();
        changeEmissionMultiplier();

        if (playerRB.velocity.magnitude >= 10f)
        {
            emission.rateOverTime = rateOverTime * emissionMultiplier;
            main.startSpeed = effectSpeed * speedMultiplier;
        }
        else
        {
            emission.rateOverTime = 0f;
        }
    }


    private void changeSpeedMultiplier()
    {
        speedMultiplier = 1+((playerRB.velocity.magnitude-10f) * 0.2f);
        speedMultiplier = Mathf.Clamp(speedMultiplier, minSpeedMultiplier, maxSpeedMultiplier);
    }
    private void changeEmissionMultiplier()
    {
        emissionMultiplier = 1 + ((playerRB.velocity.magnitude - 10f) * 0.2f);
        emissionMultiplier = Mathf.Clamp(emissionMultiplier, minRateMultiplier, maxRateMultiplier);
    }
}
