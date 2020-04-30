using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSUpdater : MonoBehaviour
{

    public Text text;
    public int fps;

    void Start()
    {
        text = GetComponent<Text>();
        StartCoroutine(UpdateFPS());
    }

    void Update()
    {
        fps = (int)(1f / Time.deltaTime);
    }

    IEnumerator UpdateFPS()
    {
        while (true)
        {
            text.text = "" + fps + " FPS";
            yield return new WaitForSeconds(0.16f);
        }
    }

}
