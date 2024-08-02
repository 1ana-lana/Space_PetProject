using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class is responsible for spawning game objects
/// </summary>
public class Spawner : MonoBehaviourSinglton<Spawner>
{
    /// <summary>
    /// event called enemy destroying
    /// </summary>
    public event Action<int> OnEnemyMurder;

    [SerializeField]
    private BonusIconSpawner bonusIconSpawner = null;

    [Serializable] public class bonusIconPair : SerializableKeyValuePair<UnitType, UnitType> { }

    [SerializeField]
    private List<bonusIconPair> bonusIconPairs = new List<bonusIconPair>();

    /// <summary>
    /// movable object spawn position 
    /// </summary>
    [SerializeField]
    private Transform spawnPosition = null;

    /// <summary>
    /// delay between asteroids
    /// </summary>
    [SerializeField]
    private int spawnDelay = 1;

    /// <summary>
    /// delay between waves of an asteroids
    /// </summary>
    [SerializeField]
    private int spawnWavesDelay = 5;

    /// <summary>
    /// minimum asteroid's count in wave
    /// </summary>
    [SerializeField]
    private int minEnemysInWave = 3;

    /// <summary>
    /// maximum asteroid's count in wave
    /// </summary>
    [SerializeField]
    private int maxEnemysInWave = 3;

    /// <summary>
    /// how often bonus will be spawn from 0 to this value
    /// </summary>
    [SerializeField]
    private float maxDelayBetweenBonus = 1f;

    private EnumInterval bonusEnumInterval = new EnumInterval(100, 103);
    private EnumInterval enemyEnumInterval = new EnumInterval(0, 2);

    /// <summary>
    /// will bonus spawn after destroy this enemy
    /// </summary>
    private bool isBonus = true;
    public bool IsBonus
    {
        get
        {
            return isBonus;
        }

        set
        {
            isBonus = value;
            if (!isBonus)
            {
                StartCoroutine(reloadBonusSpawn());
            }
        }
    }

    /// <summary>
    /// indefinitely spawns waves with different count of asteroids. 
    /// spawnWavesDelay determines delay between them. 
    /// </summary>
    private IEnumerator spawnWaves()
    {
        while (true)
        {
            int count = UnityEngine.Random.Range(minEnemysInWave, maxEnemysInWave);
            StartCoroutine(spawnWithDelay(count));
            yield return new WaitForSeconds(spawnWavesDelay);
        }
    }

    /// <summary>
    /// spawns asteroids in random positions 
    /// </summary>
    /// <param name="count">desired count of asteroids</param>
    /// <returns></returns>
    private IEnumerator spawnWithDelay(int count)
    {
        for (int i = 0; i < count; i++)
        {
            chooseRandomSpawnPosition("Asteroid");
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    /// <summary>
    /// realizes delay between bonus
    /// </summary>
    private IEnumerator reloadBonusSpawn()
    {
        float i = UnityEngine.Random.Range(0, maxDelayBetweenBonus);
        yield return new WaitForSeconds(i);
        IsBonus = true;
    }

    /// <summary>
    /// calculates random position for spawn
    /// </summary>
    private void chooseRandomSpawnPosition(string tag)
    {
        Vector2 width = new Vector2(Screen.width, 0);
        Vector2 screenWidthPoint = Camera.main.ScreenToWorldPoint(width);

        float xPosition = UnityEngine.Random.Range(-screenWidthPoint.x, screenWidthPoint.x);
        Vector3 startPoint = new Vector3(xPosition, spawnPosition.position.y, spawnPosition.position.z);

        if (tag == "Asteroid")
        {
            Vector3 direction = chooseRandomTargetPosition(screenWidthPoint.x, startPoint);
            instantiateRandomEnemyPrefab(startPoint, direction);
        }          
    }

    /// <summary>
    /// calculates target position for movement
    /// </summary>
    /// <param name="halfWidth">half of Screen Width in a world points<param>
    /// <param name="spawnPoint">spawn point</param>
    /// <returns>target direction</returns>
    private Vector3 chooseRandomTargetPosition(float halfWidth, Vector3 spawnPoint)
    {
        float xrandomPosition = UnityEngine.Random.Range(-halfWidth, halfWidth);
        Vector3 targetPoint = new Vector3(xrandomPosition, Vector3.down.y, spawnPoint.z);

        Vector3 targetVector = targetPoint - spawnPoint;
        Vector3 target = Vector3.Normalize(targetVector);

        return target;
    }

    /// <summary>
    /// spawns random asteroid from list
    /// </summary>
    /// <param name="startPoint">spawn position</param>
    private void instantiateRandomEnemyPrefab(Vector3 startPoint, Vector3 direction)
    {
        int index = UnityEngine.Random.Range(enemyEnumInterval.First, enemyEnumInterval.Last + 1);
        UnitType type = (UnitType)index;

        Asteroid asteroid = InstantiatePrefab<Asteroid>(type, startPoint);

        asteroid.Direction = direction;

        asteroid.IsBonus = IsBonus;
        if (IsBonus)
        {
            IsBonus = false;
        }

        asteroid.gameObject.SetActive(true);

        asteroid.OnMurder += asteroid_OnMurder;
    }

    public T InstantiatePrefab<T>(UnitType type, Vector3 position) where T : MonoBehaviour
    {
        T obj = Pool.Instance.Get<T>(type);
        obj.transform.position = position;
        return obj;
    }

    /// <summary>
    /// Calls OnEnemyMurder event
    /// </summary>
    /// <param name="value"></param>
    protected virtual void riseOnEnemyMurder(int value)
    {
        if (OnEnemyMurder != null) OnEnemyMurder(value);
    }

    /// <summary>
    /// will call when asteroid will be destroyed
    /// </summary>
    private void asteroid_OnMurder(DyingUnit obj)
    {      
        obj.OnMurder -= asteroid_OnMurder;

        Asteroid asteroid = obj as Asteroid;

        riseOnEnemyMurder(asteroid.Points);

        if (asteroid.IsBonus)
        {
            spawnBonus(obj.transform.position);
        }
    }

    protected virtual void spawnBonus(Vector3 position)
    {
        int index = UnityEngine.Random.Range(bonusEnumInterval.First, bonusEnumInterval.Last+1);
        Bonus bonus = InstantiatePrefab<Bonus>((UnitType)index, position);
        bonus.gameObject.SetActive(true);

        bonus.OnActiveBonus += bonus_OnActiveBonus;
    }

    private void bonus_OnActiveBonus(Bonus bonus, UnitType type, int lifeTime)
    {
        int i = bonusIconPairs.FindIndex(x => x.Key == type);

        if (i  == -1)
        {
            Debug.LogError("Icon prefab doesn't found");
            return;
        }

        bonusIconSpawner.AddBonusIcon(bonusIconPairs[i].Value, lifeTime);
        bonus.OnActiveBonus -= bonus_OnActiveBonus;
    }

    private void Start()
    {
        StartCoroutine(spawnWaves());
    }
}