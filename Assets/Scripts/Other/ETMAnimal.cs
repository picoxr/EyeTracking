using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETMAnimal : ETMEnity
{

    public override void PlayAnimation()
    {
        GetComponent<shaderGlow>().lightOn();
        switch (Name)
        {
            case "Fish":
                GetComponent<Animation>().Play("swim");
                break;
            case "Frog":
                GetComponent<Animation>().Play("jump");
                break;
            case "Chicken":
                GetComponent<Animation>().Play("walk");
                break;
            default:
                break;
        }
    }

    public override void StopAnimation()
    {
        GetComponent<shaderGlow>().lightOff();
        GetComponent<Animation>().Play("idle");
    }
    
}
