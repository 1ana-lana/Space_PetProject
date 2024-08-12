using System;
using UnityEngine;

public class Bonus : MovingUnit, IBeDestroyedAfterRange, IPoolable
{
    public event Action<Bonus, UnitType, int> OnActiveBonus;

    /// <summary>
    /// how many seconds the bonus is valid
    /// </summary>
    [SerializeField]
    private int lifeTime = 0;
    public virtual int LifeTime
    {
        get
        {
            return lifeTime;
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
    /// unique token for get object from pull
    /// </summary>
    [SerializeField]
    protected UnitType type;

    /// <summary>
    /// IPoolable field
    /// </summary>
    public UnitType Type
    {
        get
        {
            return type;
        }
    }

    void IPoolable.Off()
    {
        gameObject.SetActive(false);
    }

    void IPoolable.Reset()
    {
       
    }

    /// <summary>
    /// determine game object's movement
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

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Ship ship = collision.gameObject.GetComponent<Ship>();
            ship.UseBonus(this);
            OnActiveBonus?.Invoke(this, type, lifeTime);
            Disintegrate();
        }
        
    }

    protected override void Awake()
    {
        base.Awake();
        startPosition = transform.position;
    }

    protected void Update()
    {
        Move(new Vector3(0, -1, 0));
    }    
}
