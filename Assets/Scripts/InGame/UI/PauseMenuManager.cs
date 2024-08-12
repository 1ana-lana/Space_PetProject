using System;
using UnityEngine;

public class PauseMenuManager : GameOverMenuManager
{
    public event Action OnDisable;

    //Inspector func
    public void ResumeGame()
    {
        transform.gameObject.SetActive(false);
        OnDisable();
        Time.timeScale = 1;
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeGame();
        }
    }
}