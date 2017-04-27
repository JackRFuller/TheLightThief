using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraShakeProperties
{
    public float angle;
    public float strength;
    public float maxSpeed;
    public float minSpeed;
    public float duration;
    [Range(0, 1)]
    public float noisePercent;
    [Range(0, 1)]
    public float dampingPercent;
    [Range(0, 1)]
    public float rotationPercent;

    public CameraShakeProperties(float angle, float strength, float speed, float duration, float noisePercent, float dampingPercent, float rotationPercent)
    {
        this.angle = angle;
        this.strength = strength;
        this.maxSpeed = speed;
        this.duration = duration;
        this.noisePercent = Mathf.Clamp01(noisePercent);
        this.dampingPercent = Mathf.Clamp01(dampingPercent);
        this.rotationPercent = Mathf.Clamp01(rotationPercent);
    }
}
