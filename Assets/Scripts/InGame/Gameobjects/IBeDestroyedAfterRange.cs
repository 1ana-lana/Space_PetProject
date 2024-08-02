using UnityEngine;

/// <summary>
/// for game objects which must be destroyed if they haven't collide with target
/// </summary>
public interface IBeDestroyedAfterRange
{
    /// <summary>
    /// game object's spawn position; set on awake;
    /// </summary>
    Vector3 StartPosition {get;}

    /// <summary>
    /// maximum distance from StartPosition before destroy;
    /// </summary>
    int Range {get;}
}

