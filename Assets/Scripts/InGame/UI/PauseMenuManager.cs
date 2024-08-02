using System;
using UnityEngine;

public class PauseMenuManager : GameOverMenuManager
{
    public event Action OnDisable;

    //Inspector func
    public void ResumeGame()
    {
        transform.gameObject.SetActive(false);
        raiseOnDisable();
        Time.timeScale = 1;
    }

    private void raiseOnDisable()
    {
        if (OnDisable != null) OnDisable();
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeGame();
        }
    }
}