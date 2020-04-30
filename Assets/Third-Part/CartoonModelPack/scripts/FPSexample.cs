using UnityEngine;
using System.Collections;

public class FPSexample : MonoBehaviour {
	
	GameObject muzzleFlash;
	GameObject colt;
	
	// Use this for initialization
	void Start () 
	{
		muzzleFlash = GameObject.Find("MuzzleParticles");
		colt = GameObject.Find("PlayerColt");
	}
	
	// Update is called once per frame
	void Update () {
	  if (Input.GetMouseButtonDown(0))
		{			
			muzzleFlash.GetComponent<ParticleSystem>().Play();
			colt.GetComponent<Animation>().Play();
		}
	}
}
