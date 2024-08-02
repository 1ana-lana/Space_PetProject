using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI element for display score
/// </summary>
public class RatingCell : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform = null;
    public float Height
    {
        get
        {
            return rectTransform.rect.height;
        }
    }

    [SerializeField]
    private Text text = null;
    public string Text
    {
        get
        {
            return text.text;
        }
        set
        {
            text.text = value;
        }
    }
}