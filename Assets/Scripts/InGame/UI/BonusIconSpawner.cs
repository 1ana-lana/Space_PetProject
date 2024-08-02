using System.Collections.Generic;
using UnityEngine;

public class BonusIconSpawner : MonoBehaviour
{
    [SerializeField]
    private BonusIcon bonusIconPrefab = null;

    [SerializeField]
    private RectTransform parent = null;

    private Vector2 bonusIconStartPosition = new Vector2();

    private float bonusIconXSize = 0;

    private List<BonusIcon> activeIcons = new List<BonusIcon>();


    public void AddBonusIcon(UnitType type, int lifeTime)
    {
        if (activeIcons.Exists(x => x.Type == type))
        {
            BonusIcon temp = activeIcons.Find(x => x.Type == type);
            temp.LifeTime = lifeTime;
        }
        else
        {
            BonusIcon bonusIcon = Pool.Instance.Get<BonusIcon>(type);
            bonusIcon.transform.SetParent(parent);
            bonusIcon.RectTransform.localScale = Vector3.one;

           
            bonusIcon.LifeTime = lifeTime;
            bonusIcon.OnFinish += BonusIcon_OnFinish;

            RectTransform rt = bonusIcon.RectTransform;

            setPosition(rt);

            activeIcons.Add(bonusIcon);
        }
    }

    private void BonusIcon_OnFinish(BonusIcon obj)
    {
        obj.OnFinish += BonusIcon_OnFinish;

        if (activeIcons.Count - 1 < 0)
        {
            return;
        }

        int i = activeIcons.FindIndex(x => x == obj);
        if (i == activeIcons.Count)
        {
            return;
        }

        activeIcons.Remove(obj);

        while (i < activeIcons.Count)
        {
            float X = bonusIconStartPosition.x + i * bonusIconXSize;
            RectTransform rt = activeIcons[i].RectTransform;
            rt.localPosition = new Vector3(X, bonusIconStartPosition.y);
            i++;
        }
    }

    private void setPosition(RectTransform rt)
    {
        if (activeIcons.Count == 0)
        {
            rt.localPosition = bonusIconStartPosition;
        }
        else
        {
            float X = bonusIconStartPosition.x + activeIcons.Count * bonusIconXSize;
            rt.localPosition = new Vector3(X, bonusIconStartPosition.y);
        }
    }

    private void Start()
    {
        RectTransform rt = bonusIconPrefab.RectTransform;
        bonusIconXSize = rt.rect.width;
        float bonusIconYSize = rt.rect.height;    
        bonusIconStartPosition = new Vector2((-parent.rect.width/2) + bonusIconXSize/2, (-parent.rect.height / 2) + bonusIconYSize/2);
    }
}