using System.Collections;
using UnityEngine;

public class VisualEffect : DisintegratingUnit, IPoolable
{
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
    /// sound audio clip name 
    /// </summary>
    [SerializeField]
    private string sound = null;
    public string Sound
    {
        get
        {
            return sound;
        }
    }

    void IPoolable.Off()
    {
        gameObject.SetActive(false);
    }

    void IPoolable.Reset()
    {
       
    }

    protected override void disintegrate()
    {
        if ((object)this != null)
        {
            Pool.Instance.Add(this);
        }
    }

    private IEnumerator off()
    {
        yield return new WaitForSeconds(GetComponent<ParticleSystem>().main.duration);
        disintegrate();
    }
    private void OnEnable()
    {
        StartCoroutine(off());
    }
}