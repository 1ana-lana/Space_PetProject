using UnityEngine;

/// <summary>
/// Base class for all game objects which can move
/// </summary>
public abstract class MovingUnit : DisintegratingUnit
{
    /// <summary>
    /// game object's base speed
    /// </summary>
    [SerializeField]
    protected float baseMoveSpeed = 0;

    // Serialize for debug
    [SerializeField]
    protected float moveSpeed;
    /// <summary>
    /// game object's temporary speed; can be set from outside;
    /// </summary>
    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }

        set
        {
            moveSpeed = value;
        }
    }

    /// <summary>
    /// determine game object's movement
    /// </summary>
    /// <param name="direction"> movement target position</param>
    protected abstract void move(Vector3 direction);

    protected virtual void Awake()
    {
        moveSpeed = baseMoveSpeed;
    }
}
