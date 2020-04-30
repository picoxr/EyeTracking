using UnityEngine;
using System.Collections;

namespace Suriyun.MobileTPS
{
	public class MainCamera : MonoBehaviour
	{
	
		public Transform cam_holder;
		Transform trans;

		void Start ()
		{
			trans = transform;
		}

		void Update ()
		{
			// Smmoth out camera transition //
			trans.position = Vector3.Lerp (trans.position, cam_holder.position, 60f * Time.deltaTime);
		}
	}
}
