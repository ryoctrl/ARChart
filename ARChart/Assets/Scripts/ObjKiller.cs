using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjKiller : MonoBehaviour {

	private float lifeTime = 0.0f;
	void Start(){

	}
	
	// Update is called once per frame
	void Update () {
		lifeTime+=Time.deltaTime;
		if(lifeTime>10.0f){
			Destroy(this);
		}
	}
}
