using UnityEngine;

public class IncreaseBonus : Bonus
{
    /// <summary>
    /// how much the desired parameter will increase
    /// </summary>
    [SerializeField]
    protected float bonusValue;
    public float BonusValue
    {
        get
        {
            return bonusValue;
        }
    }
}
