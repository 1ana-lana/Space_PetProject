using System.Collections.Generic;
using UnityEngine;

public class HighScoreUI : MonoBehaviour
{
    private List<int> scores = new List<int>();

    [SerializeField]
    private RatingCell Prefab;

    [SerializeField]
    private RectTransform target;

    private void Start()
    {
        HighScoreManager.Load();

        if (HighScoreManager.Rating == null)
        {
            RatingCell ratingCell = Instantiate(Prefab, target);
            ratingCell.Text = string.Format("Plase {0} - {1}", 1, 0);
            target.sizeDelta = new Vector2(0, ratingCell.Height);
            return;
        }

        scores = HighScoreManager.Rating;

        float height = 0;

        for (int i = 0; i < scores.Count; i++)
        {
            RatingCell ratingCell = Instantiate(Prefab, target);
            ratingCell.Text = string.Format("Plase {0} - {1}", i + 1, scores[i]);
            height += ratingCell.Height;
        }

        target.sizeDelta = new Vector2(0, height);
    }
}
