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
            sortRatingList();
            removeExcess();

            if (Rating.Contains(score))
            {
                rewriteHightScore();
            }
        }
        else
        {
            Rating = new List<int>();
            Rating.Add(score);
            rewriteHightScore();
        }
    }

    /// <summary>
    /// update sequence
    /// </summary>
    private static void sortRatingList()
    {
        IEnumerable<int> temp = Rating.Distinct();
        Rating = temp.ToList();
        Rating.Sort();
        Rating.Reverse();
    }

    private static void removeExcess()
    {
        for (int i = highScoreLength; i < Rating.Count; i++)
        {
            Rating.Remove(Rating[i]);
        }
    }

    private static void rewriteHightScore()
    {
        save();
    }

    private static void save()
    {
        SaveAndLoad.Save(ratingSavePath, Rating);
    }
}
