using UnityEngine;
using TMPro;

public class FrameStatsUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textObject;

    /// <summary>
    /// The metric to display in the frame statistics panel.
    /// </summary>
    public enum DisplayMetric
    {
        FrameRate,
        FrameDuration
    };

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    DisplayMetric displayMetric;

    /// <summary>
    /// Number of seconds over which to collect frame statistics (stencil pattern).
    /// </summary>
    [SerializeField, Range(0.1f, 5f)]
    float sampleDuration = 2f;

    /// <summary>
    /// Sum of frame durations over sample duration.
    /// </summary>
    float duration = 0f;

    /// <summary>
    /// Longest frame duration and highest frame rate so far over current sample period.
    /// </summary>
    float worstDuration = float.MinValue;

    /// <summary>
    /// Shortest frame duration and lowest frame rate so far over current sample period.
    /// </summary>
    float bestDuration = float.MaxValue;

    /// <summary>
    /// Number of frames so far over sample period.
    /// </summary>
    int count = 0;

    void Update()
    {
        float currentDuration = Time.unscaledDeltaTime;
        duration += currentDuration;
        count += 1;
        worstDuration = Mathf.Max(worstDuration, currentDuration);
        bestDuration = Mathf.Min(bestDuration, currentDuration);
        if (duration >= sampleDuration)
        {
            UpdateText();
            duration = 0f;
            count = 0;
            bestDuration = float.MaxValue;
            worstDuration = float.MinValue;
        }
    }

    void UpdateText()
    {
        float avgDuration = duration / count;
        switch (displayMetric)
        {
            case DisplayMetric.FrameRate:
                textObject.SetText(
                    "FPS\n{0:0}\n{1:0}\n{2:0}",
                    1f / bestDuration,
                    1f / avgDuration,
                    1f / worstDuration
                );
                break;
            case DisplayMetric.FrameDuration:
                textObject.SetText(
                    "MS\n{0:1}\n{1:1}\n{2:1}",
                    1000f * bestDuration,
                    1000f * avgDuration,
                    1000f * worstDuration
                );
                break;
        }
    }
}
