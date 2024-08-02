using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Processes player's touches information, control ship's shots
/// </summary>
public class FireControlPanel : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// Called every time the player touches panel
    /// </summary>
    public event Action OnTouch;

    public void OnPointerClick(PointerEventData eventData)
    {
        riseOnTouch();
    }

    /// <summary>
    /// Calls OnTouch event
    /// </summary>
    protected virtual void riseOnTouch()
    {
        if (OnTouch != null) OnTouch();
    }
}