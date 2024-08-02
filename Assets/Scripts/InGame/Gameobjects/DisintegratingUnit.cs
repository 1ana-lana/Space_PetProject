using UnityEngine;

/// <summary>
/// Base class for all game objects which can be destroying
/// </summary>
public abstract class DisintegratingUnit : MonoBehaviour
{
    /// <summary>
    /// destroying game object
    /// </summary>
    protected virtual void disintegrate()
    {
        if ((object)this != null)
        {
            Destroy(gameObject);
        }
    }
}

