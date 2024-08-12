using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BonusIcon : MonoBehaviour, IPoolable
{
    public event Action<BonusIcon> OnFinish;

    [SerializeField]
    private Text text = null;

    private int lifeTime = 0;
    public int LifeTime
    {
        get
        {
            return lifeTime;
        }

        set
        {
            lifeTime = value;
        }
    }


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

    [SerializeField]
    private RectTransform rectTransform = null;
    public RectTransform RectTransform
    {
        get
        {
            return rectTransform;
        }
    }

    private IEnumerator Timer()
    {
        while (LifeTime > 0)
        {
            text.text = LifeTime + "";
            yield return new WaitForSeconds(1f);
            LifeTime -= 1;
        }

        Disintegrate();
        OnFinish?.Invoke(this);
    }

    protected virtual void Disintegrate()
    {
        if ((object)this != null)
        {
            Destroy(gameObject);
        }
    }

    void IPoolable.Off()
    {
        gameObject.SetActive(false);
    }

    void IPoolable.Reset()
    {
        
    }

    private void Start()
    {
        StartCoroutine(Timer());
    }
}