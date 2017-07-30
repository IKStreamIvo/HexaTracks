using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingImage : MonoBehaviour {

    Image image;
    public int speed = 1;

	void Start () {
        image = GetComponent<Image>();
	}
	
	void Update () {
        image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + (speed * Time.deltaTime));
        if(image.color.a == 255 || image.color.a == 0)
        {
            speed *= -1;
        }
	}
}
