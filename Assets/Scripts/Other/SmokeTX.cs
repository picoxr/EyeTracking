using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeTX : MonoBehaviour {

    // Use this for initialization
    void Start() {
        Invoke("DestroyMe",2f);
    }

    void DestroyMe() {
        Destroy(gameObject);
    }
}
