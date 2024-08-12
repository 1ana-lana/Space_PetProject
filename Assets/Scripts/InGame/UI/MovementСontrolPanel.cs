using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Processes player's touches information, control ship movement
/// </summary>
public class MovementСontrolPanel : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    /// <summary>
    /// Called every time the player touches panel
    /// </summary>
    /// <param name="Vector2">UI touch's position</param>
    /// <param name="bool">touch occurs/completed</param>
    public event Action<Vector2, bool> OnTouch;

    /// <summary>
    /// Called by the EventSystem every time the pointer is moved during dragging
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        OnTouch?.Invoke(eventData.position, true);
    }

    /// <summary>
    /// Evaluate current state and transition to pressed state
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        OnTouch?.Invoke(eventData.position, true);
    }

    /// <summary>
    /// Evaluate eventData and transition to appropriate state
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        OnTouch?.Invoke(eventData.position, false);
    }
}
