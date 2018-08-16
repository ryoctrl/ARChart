using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleStick : MonoBehaviour {

	public double highest;
	public double lowest;
	public double open;
	public double high;
	public double low;
	public double close;

	public GameObject body;

	public GameObject beard;



	// Use this for initialization
	void Start () {
		sizeInitialize();	
	}

	
	private void sizeInitialize() {
		double parentPriceRange = highest - lowest;
		double beardPriceRange = high - low;
		double bodyPriceRange = open - close;

		float parentHeight = 50;
		float beardHeight = (float) (parentHeight * (beardPriceRange / parentPriceRange));
		float bodyHeight = (float) (parentHeight * (bodyPriceRange / parentPriceRange));

		float bodyYPos = (float) (parentHeight * (open + close) / 2 / highest);
		float beardYPos = (float) (parentHeight * (high + low) / 2 / highest);

		body.transform.localScale = new Vector3(0.25f, bodyHeight, 0.25f);
		beard.transform.localScale = new Vector3(0.05f, beardHeight, 0.05f);

		Vector3 bodyPos = body.transform.position;
		bodyPos.y = bodyYPos;
		body.transform.position = bodyPos;

		Vector3 beardPos = beard.transform.position;
		beardPos.y = beardYPos;
		beard.transform.position = beardPos;

		changeCandleColor();
	}

	private void changeCandleColor() {
		Color c = open < close ? Color.green : Color.red;
		
		body.GetComponent<Renderer>().material.SetColor("_Color", c);
		beard.GetComponent<Renderer>().material.SetColor("_Color", c);
	}
}
