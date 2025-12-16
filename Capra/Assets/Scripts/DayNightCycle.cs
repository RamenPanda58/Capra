using UnityEngine;
using TMPro;

public class StaticTimeOfDay : MonoBehaviour
{
    [Header("Time of Day (Manual Slider Only)")]
    [Range(0f, 24f)]
    public float timeOfDay = 12f;

    [Header("Sun Settings")]
    public Transform sunPivot;
    public Light directionalLight;
    public Color dayColor = new Color(1f, 0.956f, 0.839f);
    public Color nightColor = new Color(0.2f, 0.3f, 0.5f);
    public float sunTilt = 23.5f;

    [Header("Ambient Light")]
    public Color ambientDayColor = Color.white;
    public Color ambientNightColor = new Color(0.1f, 0.1f, 0.2f);

    [Header("UI Clock (Optional)")]
    public TMP_Text timeText;

    private float lastTime = -1f;

    void Update()
    {
        // Only update lighting when slider value changes
        if (!Mathf.Approximately(timeOfDay, lastTime))
        {
            UpdateLighting();
            UpdateClockUI();
            lastTime = timeOfDay;
        }
    }

    private void UpdateLighting()
    {
        float normalizedTime = timeOfDay / 24f;
        float sunRotation = Mathf.Lerp(-90f, 270f, normalizedTime);

        if (sunPivot != null)
            sunPivot.localRotation = Quaternion.Euler(sunRotation, 0f, sunTilt);

        float lightT = Mathf.Clamp01(Mathf.Sin(normalizedTime * Mathf.PI * 2f));

        if (directionalLight != null)
            directionalLight.color = Color.Lerp(nightColor, dayColor, lightT);

        RenderSettings.ambientLight =
            Color.Lerp(ambientNightColor, ambientDayColor, lightT);
    }

    private void UpdateClockUI()
    {
        if (timeText == null) return;

        int h = Mathf.FloorToInt(timeOfDay);
        int m = Mathf.FloorToInt((timeOfDay - h) * 60f);
        timeText.text = $"{h:00}:{m:00}";
    }
}
