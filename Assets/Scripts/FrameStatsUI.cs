using UnityEngine;
using TMPro;

public class FrameStatsUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textObject;

    public enum DisplayMetric
    {
        FrameRate,
        FrameDuration
    };

    [SerializeField]
    DisplayMetric displayMetric;

    [SerializeField, Range(0.1f, 5f)]
    float sampleDuration = 2f;

    float duration = 0f;
    float maxDuration = float.MinValue;
    float minDuration = float.MaxValue;
    int count = 0;

    void Update()
    {
        float currentDuration = Time.unscaledDeltaTime;
        duration += currentDuration;
        count += 1;
        maxDuration = Mathf.Max(maxDuration, currentDuration);
        minDuration = Mathf.Min(minDuration, currentDuration);
        if (duration >= sampleDuration)
        {
            switch (displayMetric)
            {
                case DisplayMetric.FrameRate:
                    textObject.SetText(
                        "FPS\n{0:0}\n{1:0}\n{2:0}",
                        1f / minDuration,
                        count / duration,
                        1f / maxDuration
                    );
                    break;
                case DisplayMetric.FrameDuration:
                    textObject.SetText(
                        "MS\n{0:1}\n{1:1}\n{2:1}",
                        1000f * minDuration,
                        1000f * duration / count,
                        1000f * maxDuration
                    );
                    break;
            }
            duration = 0f;
            count = 0;
            minDuration = float.MaxValue;
            maxDuration = float.MinValue;
        }
    }
}
