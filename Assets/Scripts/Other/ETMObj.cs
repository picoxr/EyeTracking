using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETMObj : ETMEnity
{

    public override void PlayAnimation()
    {
        
        switch (Name)
        {
            case "Huey":
                transform.Find("ChopperBody").GetComponent<shaderGlow>().lightOn();
                transform.Find("BackRotor").GetComponent<SimpleRotate>().enabled = true;
                transform.Find("BigRotor").GetComponent<SimpleRotate>().enabled = true;
                break;
            case "A-10":
                transform.Find("A-10").GetComponent<shaderGlow>().lightOn();
                var jmain_1 = transform.Find("JetEngine_1").GetComponent<ParticleSystem>().main;
                jmain_1.startColor = new Color(1, 1, 1, 1);
                var jmain_2 = transform.Find("JetEngine_2").GetComponent<ParticleSystem>().main;
                jmain_2.startColor = new Color(1, 1, 1, 1);
                break;
            default:
                break;
        }
    }

    public override void StopAnimation()
    {
        switch (Name)
        {
            case "Huey":
                transform.Find("ChopperBody").GetComponent<shaderGlow>().lightOff();
                transform.Find("BackRotor").GetComponent<SimpleRotate>().enabled = false;
                transform.Find("BigRotor").GetComponent<SimpleRotate>().enabled = false;
                break;
            case "A-10":
                transform.Find("A-10").GetComponent<shaderGlow>().lightOff();
                var jmain_1 = transform.Find("JetEngine_1").GetComponent<ParticleSystem>().main;
                jmain_1.startColor = new Color(1, 1, 1, 0);
                var jmain_2 = transform.Find("JetEngine_2").GetComponent<ParticleSystem>().main;
                jmain_2.startColor = new Color(1, 1, 1, 0);
                break;
            default:
                break;
        }
    }
}
