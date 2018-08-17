using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var pos = this.transform.position;
		var deltaTime = Time.deltaTime;
		this.transform.position = new Vector3(pos.x,pos.y+0.1f*deltaTime,pos.z);
	}
}
