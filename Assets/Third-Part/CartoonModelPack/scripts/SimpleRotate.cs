using UnityEngine;
using System.Collections;

public class SimpleRotate : MonoBehaviour {

	public enum RotationAxis{
		Xaxis,Yaxis,Zaxis
	};	
	public RotationAxis selectedAxis;	
	public float speed = 1f;
	
	private Vector3 axis;
	
	// Use this for initialization
	void Start () 
	{
		switch(selectedAxis)
		{
			case RotationAxis.Xaxis:	axis = Vector3.right; 		break;	
			case RotationAxis.Yaxis:	axis = Vector3.up; 			break;	
			case RotationAxis.Zaxis:	axis = Vector3.forward; 	break;	
			default:					axis = Vector3.right; 		break;	
		}	

	}
		
	// Update is called once per frame
	void FixedUpdate () {
		transform.Rotate(axis * Time.deltaTime * speed);
	}
}
