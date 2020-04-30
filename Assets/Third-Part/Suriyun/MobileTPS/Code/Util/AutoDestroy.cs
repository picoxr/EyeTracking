using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour
{

	public float time = 3f;

	void Start ()
	{
		Destroy (this.gameObject, time);
	}
	

}
