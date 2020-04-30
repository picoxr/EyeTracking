using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETMEnity : MonoBehaviour {
    public int ID;
    //1---角色  2---动物  3---物体
    public int TYPE;
    public string Name;

    public virtual void PlayAnimation() { }

    public virtual void StopAnimation() { }
}
