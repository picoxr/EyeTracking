using UnityEngine;
using System.Collections;

namespace Suriyun.MobileTPS
{
	public class LookAt : MonoBehaviour
	{
		public Vector3 offset;
		GameObject target;

		void Start ()
		{
			target = GameObject.Find ("+target");
		}

		void Update ()
		{
			transform.LookAt (target.transform);
			transform.Rotate (offset);
		}
	}

}