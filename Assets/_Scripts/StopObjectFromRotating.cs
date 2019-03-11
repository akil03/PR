using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopObjectFromRotating : MonoBehaviour {

    public Vector3 angle;
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.Euler(angle);
	}
}
