using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Suriyun.MobileTPS
{
    public class UnityUIButton : Button, IPointerDownHandler, IPointerUpHandler
    {

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log(
                "Down : " + eventData.pointerId +
                "\nPosition : " + eventData.position
            );
            pressed = true;
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log(
                "Up : " + eventData.pointerId +
                "\nPosition : " + eventData.position
            );
            pressed = false;
        }
    }
}