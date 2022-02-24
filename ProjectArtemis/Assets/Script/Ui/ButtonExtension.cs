using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonExtension : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent OnHoverEnter;
    public UnityEvent OnHoverExit;
    public UnityEvent OnButtonSelect;
    public UnityEvent OnButtonDeSelect;

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverEnter?.Invoke();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        OnHoverExit?.Invoke();
    }
    public void OnSelect(BaseEventData eventData)
    {
        OnButtonSelect?.Invoke();
    }
    public void OnDeselect(BaseEventData eventData)
    {
        OnButtonDeSelect?.Invoke();
    }
}
