using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcreteBlock : MonoBehaviour {

	public void ChangeMat(Material mat)
    {
        GetComponentInChildren<MeshRenderer>().material = mat;
    }
}
