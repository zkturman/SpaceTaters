using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterBehaviour : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem leftThruster;
    [SerializeField]
    private ParticleSystem rightThruster;
    [SerializeField]
    [Range(15f, 18f)]
    private float maxThrustEmission = 15f;
    [SerializeField]
    [Range(10f, 12f)]
    private float restingThrustEmission = 12f;
    [SerializeField]
    [Range(6f, 8f)]
    private float minThrustEmission = 8f;
    [SerializeField]
    [Range(0.3f, 0.4f)]
    private float maxThrustParticleSize = 15f;
    [SerializeField]
    [Range(0.2f, 0.25f)]
    private float restingThrustParticleSize = 12f;
    [SerializeField]
    [Range(0.1f, 0.15f)]
    private float minThrustParticleSize = 8f;
    [SerializeField]
    private Color maxThrustColor;
    [SerializeField]
    private Color restingThrustColor;
    [SerializeField]
    private Color minThrustColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isLeftThrusterActive())
        {
            setThrusterPower(leftThruster, rightThruster);
        }
        else if (isRightThrusterActive())
        {
            setThrusterPower(rightThruster, leftThruster);
        }
        else
        {
            resetThrusters();
        }
    }

    private bool isLeftThrusterActive()
    {
        return InputController.IsKeyInGroupPressed(InputController.RightKeyCodes);
    }

    private bool isRightThrusterActive()
    {
        return InputController.IsKeyInGroupPressed(InputController.LeftKeyCodes);
    }

    private void setThrusterPower(ParticleSystem activeThruster, ParticleSystem inactiveThruster)
    {
        setThrusterEmission(activeThruster, maxThrustEmission);
        setThrusterParticleSize(activeThruster, maxThrustParticleSize);
        setThrusterColor(activeThruster, maxThrustColor);
        setThrusterEmission(inactiveThruster, minThrustEmission);
        setThrusterParticleSize(inactiveThruster, minThrustParticleSize);
        setThrusterColor(inactiveThruster, minThrustColor);
    }

    private void setThrusterEmission(ParticleSystem thrusterToChange, float newEmission)
    {
        var thrusterEmission = thrusterToChange.emission;
        thrusterEmission.rateOverTime = newEmission;
    }

    private void setThrusterParticleSize(ParticleSystem thrusterToChange, float newSize)
    {
        var thrusterMainSettings = thrusterToChange.main;
        thrusterMainSettings.startSize = newSize;
    }

    private void setThrusterColor(ParticleSystem thrusterToChange, Color newColor)
    {
        var thrusterMainSettings = thrusterToChange.main;
        thrusterMainSettings.startColor = newColor;
    }

    private void resetThrusters()
    {
        if (!isThrusterResting(leftThruster))
        {
            setThrusterEmission(leftThruster, restingThrustEmission);
            setThrusterParticleSize(leftThruster, restingThrustParticleSize);
            setThrusterColor(leftThruster, restingThrustColor);
            
        }
        if (!isThrusterResting(rightThruster))
        {
            setThrusterEmission(rightThruster, restingThrustEmission);
            setThrusterParticleSize(rightThruster, restingThrustParticleSize);
            setThrusterColor(rightThruster, restingThrustColor);
        }
    }

    private bool isThrusterResting(ParticleSystem thrusterToCheck)
    {
        var thrusterEmission = thrusterToCheck.emission;
        bool isEmissionNormal = Mathf.Approximately(thrusterEmission.rateOverTime.constant, restingThrustEmission);
        var thrusterMainSettings = thrusterToCheck.main;
        bool isSizeNormal = Mathf.Approximately(thrusterMainSettings.startSize.constant, restingThrustParticleSize);
        bool isColorNormal = thrusterMainSettings.startColor.color == restingThrustColor;
        return isEmissionNormal && isSizeNormal && isColorNormal;
    }

    public void SetThrusterScale(float scaleFactor)
    {
        leftThruster.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        rightThruster.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }
}
