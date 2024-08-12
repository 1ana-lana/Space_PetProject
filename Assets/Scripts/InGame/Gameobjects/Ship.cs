using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : DyingUnit
{
    /// <summary>
    /// active bullet type 
    /// </summary>
    protected UnitType activeBullet = UnitType.Bullet;

    /// <summary>
    /// time for reload weapon
    /// </summary>
    [SerializeField]
    protected float baseReloadTime = 0;

    protected float reloadTime = 0.2f;

    protected bool immortal = false;

    protected Vector3 moveTarget;

    protected bool moveActive = false;

    public override float HitPoints
    {
        get
        {
            return hitPoints;
        }

        set
        {
            if (isAlive)
            {
                if (!immortal || value > HitPoints)
                {
                    hitPoints = value;

                    if (value < 0)
                    {
                        Death();
                    }
                }
            }
        }
    }

    protected Dictionary<UnitType, Coroutine> bonusEffects = new Dictionary<UnitType, Coroutine>();

    /// <summary>
    /// left screen border
    /// </summary>
    private float minX;
    /// <summary>
    /// right screen border
    /// </summary>
    private float maxX;

    /// <summary>
    /// Is ship able to shoot
    /// </summary>
    protected bool isReload = true;
    public bool IsReload
    {
        get
        {
            return isReload;
        }

        set
        {
            isReload = value;
            if (!isReload)
            {
                StartCoroutine(Reload());
            }
        }
    }

    public void UseBonus(Bonus bonus)
    {
        if ((object)this != null)
        {
            Coroutine effectCoroutine;
            if (bonusEffects.TryGetValue(bonus.Type, out effectCoroutine))
            {
                bonusEffects.Remove(bonus.Type);
                StopCoroutine(effectCoroutine);
            }

            switch (bonus.Type)
            {
                case UnitType.MoveSpeedBonus:
                    MoveSpeed += (bonus as IncreaseBonus).BonusValue;
                    effectCoroutine = StartCoroutine(DifferedAction(bonus.LifeTime, delegate ()
                    {
                        MoveSpeed = baseMoveSpeed;
                    }));
                    break;
                case UnitType.ImmortalBonus:
                    immortal = true;
                    effectCoroutine = StartCoroutine(DifferedAction(bonus.LifeTime, delegate ()
                    {
                        immortal = false;
                    }));
                    break;
                case UnitType.ReloadBonus:
                    reloadTime = (bonus as IncreaseBonus).BonusValue;
                    effectCoroutine = StartCoroutine(DifferedAction(bonus.LifeTime, delegate ()
                    {
                        reloadTime = baseReloadTime;
                    }));
                    break;
                case UnitType.FireBallBonus:
                    activeBullet = UnitType.FireBall;
                    effectCoroutine = StartCoroutine(DifferedAction(bonus.LifeTime, delegate ()
                    {
                        activeBullet = UnitType.Bullet;
                    }));
                    break;
                default:
                    break;
            }

            bonusEffects.Add(bonus.Type, effectCoroutine);
        }
    }

    /// <summary>
    /// activates shot
    /// </summary>
    public void Shot()
    {
        if (IsReload)
        {
            Bullet bullet = Spawner.Instance.InstantiatePrefab<Bullet>(activeBullet, transform.position);    
            bullet.gameObject.SetActive(true);            

            SoundManager.Instance.PlaySound(bullet.ShotSoundName); 
            IsReload = false;
        }
    }

    /// <summary>
    /// called when MovementСontrolPanel's OnTouch event activated
    /// ship was subscribed to this event in GameController 
    /// </summary>
    /// <param name="touchPosition">UI touch's position</param>
    /// <param name="isTouch"> is player touching the panel</param>
    public void MoveTo(Vector2 touchPosition, bool isTouch)
    {
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        playerScreenPoint = new Vector3(touchPosition.x, playerScreenPoint.y, playerScreenPoint.z);

        moveTarget = Camera.main.ScreenToWorldPoint(playerScreenPoint);
        moveActive = true;
    }

    /// <summary>
    /// activates weapon reload
    /// </summary>
    /// <returns></returns>
    protected IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        IsReload = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="call"></param>
    protected IEnumerator DifferedAction(float delay, Action call)
    {
        yield return new WaitForSeconds(delay);
        call();
    }

    protected override void Move(Vector3 target)
    {
        if (transform.position == target)
        {
            moveActive = false;
        }
        else
        {
            Vector3 temp = new Vector3(target.x, transform.position.y, transform.position.z);
            float step = MoveSpeed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, temp, step);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        float height = Camera.main.orthographicSize * 2.0f;
        float width = height * Camera.main.aspect;

        float temp = GetComponent<CircleCollider2D>().radius / 2f;
        minX = (-width / 2f) + temp;
        maxX = width / 2f - temp;
    }

    private void Update()
    {
        if (moveActive)
        {
            Move(moveTarget);
        }

        if (transform.position.x < minX)
        {
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > maxX)
        {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shot();
        }
    }
}
