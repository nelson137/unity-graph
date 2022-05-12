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
    /// The average duration of the current sample period.
    /// </summary>
    float sampleAverageDuration;

    /// <summary>
    /// The best (lowest) duration of the current sample period.
    /// </summary>
    float sampleBestDuration;

    /// <summary>
    /// The worst (highest) duration of the current sample period.
    /// </summary>
    float sampleWorstDuration;

    /// <summary>
    /// Switch to the next metric showing the same stats as currently shown.
    /// </summary>
    public void NextMetric()
    {
        switch (displayMetric)
        {
            case DisplayMetric.FrameRate:
                displayMetric = DisplayMetric.FrameDuration;
                break;
            case DisplayMetric.FrameDuration:
                displayMetric = DisplayMetric.FrameRate;
                break;
        }
        SetText();
    }

    /// <summary>
    /// Set the text with the correct format according to the current display metric.
    /// </summary>
    void SetText()
    {
        switch (displayMetric)
        {
            case DisplayMetric.FrameRate:
                textObject.SetText(
                    "FPS\n{0:0}\n{1:0}\n{2:0}",
                    1f / sampleBestDuration,
                    1f / sampleAverageDuration,
                    1f / sampleWorstDuration
                );
                break;
            case DisplayMetric.FrameDuration:
                textObject.SetText(
                    "MS\n{0:1}\n{1:1}\n{2:1}",
                    1000f * sampleBestDuration,
                    1000f * sampleAverageDuration,
                    1000f * sampleWorstDuration
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
            // Cache stats from sample
            sampleAverageDuration = duration / count;
            sampleBestDuration = bestDuration;
            sampleWorstDuration = worstDuration;

            // Update text
            SetText();

            // Reset aggregation variables
            duration = 0f;
            count = 0;
            bestDuration = float.MaxValue;
            worstDuration = float.MinValue;
        }
    }
}
