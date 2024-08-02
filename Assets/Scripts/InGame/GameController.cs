using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class has links to game objects and communicates between them
/// </summary>
public class GameController : MonoBehaviour
{
    /// <summary>
    /// instantiated ship's link 
    /// </summary>
    [SerializeField]
    private Ship ship = null;

    /// <summary>
    /// instantiated movement control panel's link 
    /// </summary>
    [SerializeField]
    private MovementСontrolPanel movementСontrolPanel = null;

    /// <summary>
    /// instantiated ui controller's link 
    /// </summary>
    [SerializeField]
    private UIController uiController = null;

    /// <summary>
    /// instantiated fire control panel's link 
    /// </summary>
    [SerializeField]
    private FireControlPanel fireControlPanel = null;

    private List<int> rating = new List<int>();

    private bool getRecord = false;

    /// <summary>
    /// is slow motion mode active
    /// </summary>
    private bool isSlowMotion = false;
    public bool IsSlowMotion
    {
        get
        {
            return isSlowMotion;
        }

        set
        {
            if (value != isSlowMotion)
            {
                if (value)
                {
                    Time.timeScale = 0.5f;
                    SoundManager.Instance.SoundSpeed = 0.5f;
                }
                else
                {
                    Time.timeScale = 1;
                    SoundManager.Instance.SoundSpeed = 1;                    
                }

                isSlowMotion = value;
            }
        }
    }


    /// <summary>
    /// how many points player has got for all game
    /// </summary>
    [SerializeField]
    private int score = 0;

    private void instance_OnEnemyMurder(int value)
    {
        score += value;
        uiController.UpdateScore(score);
        checkrecord();
    }

    private void checkrecord()
    {
        if (getRecord)
        {
            return;
        }

        if (rating==null)
        {            
            return;
        }

        if (rating[0] < score)
        {
            uiController.NewRecord();
            getRecord = true;
            HighScoreManager.UpdateRatingList(score);
        }
    }

    private void Awake()
    {
        movementСontrolPanel.OnTouch += ship.MoveTo;
        fireControlPanel.OnTouch += ship.Shot;

        movementСontrolPanel.OnTouch += MovementСontrolPanel_OnTouch;

        Spawner.Instance.OnEnemyMurder += instance_OnEnemyMurder;
        ship.OnMurder += Ship_OnMurder;

        rating = HighScoreManager.Rating;
    }

    private void Ship_OnMurder(DyingUnit obj)
    {
        movementСontrolPanel.OnTouch -= ship.MoveTo;
        fireControlPanel.OnTouch -= ship.Shot;

        uiController.ActiveGameOverMenu();

        HighScoreManager.UpdateRatingList(score);
    }

    private void MovementСontrolPanel_OnTouch(Vector2 arg1, bool value)
    {
        IsSlowMotion = !value;
        uiController.IsTouch = value;
    }
}
