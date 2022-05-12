using UnityEngine;
using TMPro;

/// <summary>
/// The metric to display in the frame statistics panel.
/// </summary>
public enum DisplayMetric
{
    /// <summary>
    /// Show frame statistics as FPS.
    /// </summary>
    FrameRate,

    /// <summary>
    /// Show frame statistics as duration in milliseconds.
    /// </summary>
    FrameDuration
};

/// <summary>
/// Show frame statistics on the canvas.
/// </summary>
public class FrameStatsUI : MonoBehaviour
{
    /// <summary>
    /// The text object for displaying the frame statistics.
    /// </summary>
    [SerializeField]
    TextMeshProUGUI textObject;

    /// <summary>
    /// The current metric used to display the frame statistics.
    /// <br />
    /// See <seealso cref="DisplayMetric"/>
    /// for possible values.
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

    /// <summary>
    /// Zero out all stats for the next sample duration.
    /// </summary>
    void ResetStats()
    {
        duration = 0f;
        count = 0;
        bestDuration = float.MaxValue;
        worstDuration = float.MinValue;
    }

    /// <summary>
    /// Cycle to the next metric. This will zero out the stats and restart the period.
    /// </summary>
    public void NextMetric()
    {
        switch (displayMetric)
        {
            case DisplayMetric.FrameRate:
                displayMetric = DisplayMetric.FrameDuration;
                SetText(0f, 0f, 0f);
                break;
            case DisplayMetric.FrameDuration:
                displayMetric = DisplayMetric.FrameRate;
                SetText(float.MaxValue, float.MaxValue, float.MaxValue);
                break;
        }
    }

    /// <summary>
    /// Set the text with the correct format according to the current display metric.
    /// </summary>
    /// <param name="bestDur">The best (lowest) duration of the current sample period.</param>
    /// <param name="avgDur">The average duration of the current sample period.</param>
    /// <param name="worstDur">The worst (highest) duration of the current sample period.</param>
    void SetText(float bestDur, float avgDur, float worstDur)
    {
        switch (displayMetric)
        {
            case DisplayMetric.FrameRate:
                textObject.SetText(
                    "FPS\n{0:0}\n{1:0}\n{2:0}",
                    1f / bestDur,
                    1f / avgDur,
                    1f / worstDur
                );
                break;
            case DisplayMetric.FrameDuration:
                textObject.SetText(
                    "MS\n{0:1}\n{1:1}\n{2:1}",
                    1000f * bestDur,
                    1000f * avgDur,
                    1000f * worstDur
                );
                break;
            default:
                textObject.SetText("N/A\n0\n0\n0");
                break;
        }
    }

    void Update()
    {
        // Aggregate the frame stats.
        float currentDuration = Time.unscaledDeltaTime;
        duration += currentDuration;
        count += 1;
        worstDuration = Mathf.Max(worstDuration, currentDuration);
        bestDuration = Mathf.Min(bestDuration, currentDuration);
        // If at the end of one sample duration, show the aggregated stats and reset.
        if (duration >= sampleDuration)
        {
            float avgDuration = duration / count;
            SetText(bestDuration, avgDuration, worstDuration);
            ResetStats();
        }
    }
}
