using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingLight : MonoBehaviour {

    Light _light;
    public float speed = 2f;
    int dir = 1;
    public Vector2 minMax;
    public float speedR = 2f;
    int dirR = 1;
    public Vector2 minMaxR;

    void Start ()
    {
        _light = GetComponent<Light>();
        speedR = Random.Range(1.00f, 3.00f);
    }
	
	void Update ()
    {
        _light.range += dirR * speedR * Time.deltaTime;
        if (_light.range >= minMaxR.y || _light.range <= minMaxR.x)
        {
            dirR *= -1;
            speedR = Random.Range(1.00f, 3.00f);
        }

    }
}
