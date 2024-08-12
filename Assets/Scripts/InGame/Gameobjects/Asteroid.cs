using System;
using UnityEngine;

public class Asteroid : DyingUnit, IPoolable, IBeDestroyedAfterRange
{
    /// <summary>
    /// will bonus spawn after destroy this enemy
    /// </summary>
    [NonSerialized]
    public bool IsBonus = false;

    /// <summary>
    /// how many hit points unit can take away at once
    /// </summary>
    [SerializeField]
    protected float damage = 5;

    [SerializeField]
    protected bool getDamage = false;

    /// <summary>
    /// animation explosion
    /// </summary>
    [SerializeField]
    protected VisualEffect explosion = null;

    /// <summary>
    /// how many points will get player if he destroys this object
    /// </summary>
    [SerializeField]
    protected int points;
    public int Points
    {
        get
        {
            return points;
        }
    }

    /// <summary>
    /// unique token for get object from pull
    /// </summary>
    [SerializeField]
    private UnitType type;
    UnitType IPoolable.Type
    {
        get
        {
            return type;
        }
    }

    /// <summary>
    /// movement direction; assigned in class Spawner
    /// </summary>
    private Vector3 direction;
    public Vector3 Direction
    {
        set
        {
            direction = value;
        }
    }

    /// <summary>
    /// game object's spawn position; set on awake;
    /// </summary>
    protected Vector3 startPosition;
    public Vector3 StartPosition
    {
        get
        {
            return startPosition;
        }
    }

    /// <summary>
    /// maximum distance from StartPosition before destroy;
    /// </summary>
    [SerializeField]
    protected int range = 200;
    public int Range
    {
        get
        {
            return range;
        }
    }

    /// <summary>
    /// Makes the object inaccessible to interaction.
    /// Function for pull. 
    /// </summary>
    void IPoolable.Off()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Returns object to base state.
    /// Function for pull.
    /// </summary>
    void IPoolable.Reset()
    {
        moveSpeed = baseMoveSpeed;
        hitPoints = baseHitPoints;

        isAlive = true;
        getDamage = false;
    }

    public override void SetDamage(float damage)
    {
        if (!isAlive)
        {
            return;
        }

        if (getDamage)
        {
            HitPoints -= damage;
        }
    }

    /// <summary>
    /// determines game object's movement
    /// </summary>
    /// <param name="direction"> movement target position</param>
    protected override void Move(Vector3 direction)
    {
        Vector3 moveSpeedVector3 = direction * (moveSpeed * Time.deltaTime);
        transform.position += moveSpeedVector3;

        if (Vector3.Distance(transform.position, startPosition) >= range)
        {
            Disintegrate();
        }
    }

    protected override void Disintegrate()
    {
        if ((object)this != null)
        {
            Pool.Instance.Add(this);
        }
    }

    protected override void Death()
    {
        isAlive = false;

        VisualEffect effect = Spawner.Instance.InstantiatePrefab<VisualEffect>(explosion.Type, transform.position);
        effect.gameObject.SetActive(true);
        SoundManager.Instance.PlaySound(explosion.Sound);

        CallOnMurderEvent(this);
        Disintegrate();
    }

    /// <summary>
    /// determines behavior when colliding with another gameobject
    /// 2D for haven't problem with scene's deep
    /// </summary>
    /// <param name="collision">collider of other gameobject</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Ship>().SetDamage(damage); 
        }
        else if (collision.tag == "Bullet")
        {
            float bulletDamage = collision.GetComponent<Bullet>().GetDamage();
            SetDamage(bulletDamage);
        }
        else if (collision.tag == "Border")
        {
            getDamage = true;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        startPosition = transform.position;
    }

    protected void Update()
    {
        Move(direction);
    }
}