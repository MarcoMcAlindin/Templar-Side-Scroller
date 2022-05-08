using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool _buttonPressed;


    public void OnPointerDown(PointerEventData eventData)
    {
        _buttonPressed = true;

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _buttonPressed = false;
    }
}
