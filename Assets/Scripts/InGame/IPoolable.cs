/// <summary>
/// For objects are able to add to the Poll
/// </summary>
public interface IPoolable
{
    /// <summary>
    /// object's type
    /// </summary>
    UnitType Type { get; }

    /// <summary>
    /// Makes the object inaccessible to interaction.
    /// </summary>
    void Off();

    /// <summary>
    /// Returns object to base state.
    /// </summary>
    void Reset();
}

/// <summary>
/// object's types for objects one class
/// </summary>
public enum UnitType
{
    AsteroidFirst = 0,
    AsteroidSecond = 1,
    AsteroidThird = 2,
    FireBallBonus = 100,
    ImmortalBonus = 101,
    ReloadBonus = 102,
    MoveSpeedBonus = 103,
    Bullet = 200,
    FireBall = 201,
    Explosion = 300,
    FireBallBonusIcon = 400,
    ImmortalBonusIcon = 401,
    ReloadBonusIcon = 402,
    MoveSpeedBonusIcon = 403,
}