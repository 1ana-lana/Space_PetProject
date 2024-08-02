using UnityEngine;

public class BackGroundControler : MonoBehaviour
{
    /// <summary>
    /// game scene background
    /// </summary>
    [SerializeField]
    private Transform background = null;

    /// <summary>
    /// background's scroll speed
    /// </summary>
    [SerializeField]
    private float scrollSpeed = 0;

    /// <summary>
    ///background default position; set on awake;
    /// </summary>
    private Vector3 backgroundStartPosition;

    private void Start()
    {
        backgroundStartPosition = background.transform.position;
    }

    private void Update()
    {
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, background.lossyScale.y);
        background.position = backgroundStartPosition + Vector3.up * newPosition;
    }
}
