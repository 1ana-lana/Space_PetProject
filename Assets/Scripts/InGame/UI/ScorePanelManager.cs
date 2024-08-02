using UnityEngine;

public class ScorePanelManager : MonoBehaviour
{
    [SerializeField]
    private RatingCell bestScore = null;
    [SerializeField]
    private RatingCell currentScore = null;

    public void Active(int currentValue)
    {
        currentScore.Text = currentValue + "";
        gameObject.SetActive(true);
    }

    private void Start()
    {
        bestScore.Text = HighScoreManager.Rating[0] + "";
    }
}