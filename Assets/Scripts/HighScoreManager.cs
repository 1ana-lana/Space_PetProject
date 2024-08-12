using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Store and update high score raiting
/// </summary>
public static class HighScoreManager
{
    [SerializeField]
    private static int highScoreLength = 10;

    [SerializeField]
    private static string ratingSavePath = "/save";

    public static List<int> Rating = new List<int>();

    public static void Load()
    {
        Rating = SaveAndLoad.Load<List<int>>(ratingSavePath);
    }

    public static void UpdateRatingList(int score)
    {
        if (Rating != null)
        {
            Rating.Add(score);
            SortRatingList();
            RemoveExcess();

            if (Rating.Contains(score))
            {
                RewriteHightScore();
            }
        }
        else
        {
            Rating = new List<int>();
            Rating.Add(score);
            RewriteHightScore();
        }
    }

    /// <summary>
    /// update sequence
    /// </summary>
    private static void SortRatingList()
    {
        IEnumerable<int> temp = Rating.Distinct();
        Rating = temp.ToList();
        Rating.Sort();
        Rating.Reverse();
    }

    private static void RemoveExcess()
    {
        for (int i = highScoreLength; i < Rating.Count; i++)
        {
            Rating.Remove(Rating[i]);
        }
    }

    private static void RewriteHightScore()
    {
        Save();
    }

    private static void Save()
    {
        SaveAndLoad.Save(ratingSavePath, Rating);
    }
}
