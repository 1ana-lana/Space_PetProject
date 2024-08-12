using UnityEngine;

/// <summary>
/// Base class for bullets
/// </summary>
public class Bullet : MovingUnit, IPoolable, IBeDestroyedAfterRange
{
    /// <summary>
    /// how many hit points unit can take away at once
    /// </summary>
    [SerializeField]
    protected float damage = 5;

    /// <summary>
    /// unique token for get object from pull
    /// </summary>
    [SerializeField]
    private UnitType type;

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
    /// shot's sound audio clip name 
    /// </summary>
    [SerializeField]
    private string shotSoundName = "";
    public string ShotSoundName
    {
        get
        {
            return shotSoundName;
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
    /// return damage, destroy bullet
    /// </summary>
    /// <returns>bullet's damage</returns>
    public virtual float GetDamage()
    {
        Disintegrate();
        return damage;
    }

    protected override void Disintegrate()
    {
        if ((object)this != null)
        {
            Pool.Instance.Add(this);
        }
    }

    /// <summary>
    /// determine game object's movement
    /// </summary>
    /// <param name="target"> movement target position</param>
    protected override void Move(Vector3 target)
    {
        Vector3 moveSpeedVector3 = target * (MoveSpeed * Time.deltaTime);
        transform.position += moveSpeedVector3;

        if (Vector3.Distance(transform.position, startPosition) >= range)
        {
            Disintegrate();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        startPosition = transform.position;
    }
    protected void OnEnable()
    {
        startPosition = transform.position;
    }

    protected void Update()
    {        
        Move(new Vector3(0, 1, 0));
    }
}
