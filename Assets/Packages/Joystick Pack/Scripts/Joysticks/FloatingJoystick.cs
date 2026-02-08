using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    private Vector2 _startPosition;
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        _startPosition = background.anchoredPosition;
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.anchoredPosition = _startPosition;
        base.OnPointerUp(eventData);
    }
}