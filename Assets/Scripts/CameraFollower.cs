using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {

    public Transform target;
    public float moveSpeed = 5f;

	void Start () {
        target = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Update () {
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
	}
}
