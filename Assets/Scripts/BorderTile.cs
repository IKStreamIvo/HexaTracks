using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderTile : MonoBehaviour {

	// Use this for initialization
	void Start () {
        /*int rnd = Random.Range(0, 6);
        if (rnd == 0)
        {
            transform.Rotate(new Vector3()
        }*/
        transform.Rotate(new Vector3(0, Random.Range(0, 5) * 60f, 0));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
