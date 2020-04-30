using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETMCharacter : ETMEnity {

    //private void Start()
    //{
    //    PlayAnimation();
    //}
    public override void PlayAnimation()
    {
        //GetComponent<shaderGlow>().lightOn();
        GetComponent<Animation>().Play("trigger");
    }

    public override void StopAnimation()
    {
        //GetComponent<shaderGlow>().lightOff();
        GetComponent<Animation>().Play("nor");
    }
}
