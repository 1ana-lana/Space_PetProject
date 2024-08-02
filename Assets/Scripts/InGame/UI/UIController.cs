using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject upPanel = null;

    [SerializeField]
    private Text scoreTxt = null;

    [SerializeField]
    private PauseMenuManager pauseMenuPanel = null;
    [SerializeField]
    private GameOverMenuManager gameOverMenuPanel = null;
    [SerializeField]
    private ScorePanelManager scorePanel = null;

    private bool isBlinking = false;

    private int score = 0;

    private bool isTouch = false;
    public bool IsTouch
    {
        set
        {
            isTouch = value;
            setPanelState();
        }
    }

    //Inspector func
    public void ActivePauseMenu()
    {
        pauseMenuPanel.gameObject.SetActive(true);
        scorePanel.Active(score);
        Time.timeScale = 0;
    }

    public void ActiveGameOverMenu()
    {
        gameOverMenuPanel.gameObject.SetActive(true);
        scorePanel.Active(score);
        Time.timeScale = 0;
    }

    public void UpdateScore(int value)
    {
        score = value;
        scoreTxt.text = value + "";

        if (isBlinking)
        {
            return;
        }
    }

    public void NewRecord()
    {       
        StartCoroutine(blinkScore());
    }

    private IEnumerator blinkScore()
    {
        float i = 1f;

        isBlinking = true;
        upPanel.SetActive(true);

        while (i > 0)
        {
            scoreTxt.enabled = false;
            yield return new WaitForSeconds(0.2f);
            scoreTxt.enabled = true;
            yield return new WaitForSeconds(0.2f);
            i -= 0.2f;
        }

        scoreTxt.enabled = true;
        isBlinking = false;
        setPanelState();
    }

    private void PauseMenuPanel_OnDisable()
    {
        scorePanel.gameObject.SetActive(false);
    }

    private void setPanelState()
    {
        if (isBlinking)
        {
            return;
        }

        if (isTouch)
        {
            if (upPanel.activeSelf)
            {
                upPanel.SetActive(false);
            }
        }
        else
        {
            if (!upPanel.activeSelf)
            {
                upPanel.SetActive(true);
            }
        }
    }

    private void Start()
    {
        pauseMenuPanel.OnDisable += PauseMenuPanel_OnDisable;
    }
}