using UnityEngine;
using System.Collections;

namespace Suriyun.MobileTPS
{
	public class TouchScriptButtonFire : TouchScriptButton
	{
        [HideInInspector]
		public Agent agent;

		protected override void Press_Pressed (object sender, System.EventArgs e)
		{
			base.Press_Pressed (sender, e);
            if (agent == null)
            {
                agent = GameObject.FindObjectOfType<Agent>();
            }
			agent.behaviour.StartFiring ();
		}

		protected override void Release_Released (object sender, System.EventArgs e)
		{
			base.Release_Released (sender, e);
			agent.behaviour.StopFiring ();
		}
	}
}
