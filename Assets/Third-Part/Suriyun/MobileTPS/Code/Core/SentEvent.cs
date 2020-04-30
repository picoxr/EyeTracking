using UnityEngine;
using System.Collections;

namespace Suriyun.MobileTPS
{
	public class SentEvent : StateMachineBehaviour
	{
		static Agent agent;

		override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (agent == null) {
				agent = animator.GetComponent<Agent> ();
			}

			// Sync mecanime state with outside variables //s
			if (stateInfo.IsName ("hide")) {
				agent.game_camera.zoomed = false;
			} else if (stateInfo.IsName ("run")) {
				agent.game_camera.zoomed = false;
			} else if (stateInfo.IsName ("shoot")) {
				agent.game_camera.zoomed = true;
			}
		}
	}
}
