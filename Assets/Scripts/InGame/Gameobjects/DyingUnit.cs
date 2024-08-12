using System;
using UnityEngine;

/// <summary>
/// Base class for all game objects which can die because of losing points.
/// </summary>
public abstract class DyingUnit : MovingUnit
{
    /// <summary>
    /// event called gameobject destroying
    /// </summary>
    public event Action<DyingUnit> OnMurder;

    /// <summary>
    /// additional verification of game object state
    /// </summary>
    protected bool isAlive = true;

    /// <summary>
    /// unit's base hitPoints
    /// </summary>
    [SerializeField]
    protected float baseHitPoints;

    protected float hitPoints;
    /// <summary>
    /// unit's temporary hitPoints; can be set outside
    /// </summary>
    public virtual float HitPoints
    {
        get
        {
            return hitPoints;
        }

        set
        {
            if (isAlive)
            {
                if (value <= 0)
                {
                    Death();
                }
                else
                {
                    hitPoints = value;
                }
            }
        }
    }

    /// <summary>
    /// set damge to this unit
    /// </summary>
    /// <param name="damage">how many hit points this unit will lose</param>
    public virtual void SetDamage(float damage)
    {
        if (isAlive)
        {
            HitPoints -= damage;
        }
    }

    /// <summary>
    /// make this unit die
    /// </summary>
    protected virtual void Death()
    {
        isAlive = false;
        CallOnMurderEvent(this);
        Disintegrate();
    }

    protected void CallOnMurderEvent(DyingUnit dyingUnit)
    {
        OnMurder?.Invoke(dyingUnit);
    }

    protected override void Awake()
    {
        base.Awake();
        hitPoints = baseHitPoints;
    }
}