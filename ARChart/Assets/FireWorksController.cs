using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorksController : MonoBehaviour
{
	[SerializeField] private ParticleSystem[] particleSystem;

	// Use this for initialization
	void Start()
	{
		ParticleSystem.MinMaxGradient color = new ParticleSystem.MinMaxGradient();
		color.mode = ParticleSystemGradientMode.Color;
		color.color = new Color(Random.value, Random.value, Random.value, 1.0f);
		foreach (var particle in particleSystem){
			ParticleSystem.MainModule main = particle.main;
			main.startColor = color;
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}
