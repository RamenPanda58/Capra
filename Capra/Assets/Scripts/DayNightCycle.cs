using UnityEngine;
using TMPro;

public class DayNightCycle : MonoBehaviour
{
    [Header("Time of Day (0–24 hours)")]
    [Range(0f, 24f)]
    public float timeOfDay = 12f;   // Controlled by slider

    [Header("Sun Settings")]
    public Transform sunPivot;
    public Light directionalLight;
    public Color dayColor = new Color(1f, 0.956f, 0.839f);
    public Color nightColor = new Color(0.2f, 0.3f, 0.5f);
    public float sunTilt = 23.5f;

    [Header("Ambient Light Settings")]
    public Color ambientDayColor = Color.white;
    public Color ambientNightColor = new Color(0.1f, 0.1f, 0.2f);

    [Header("Villager Settings (optional)")]
    public GameObject villagersObject;
    public float villagerStartHour = 6.5f;
    public float villagerEndHour = 20f;

    [Header("UI Clock (optional)")]
    public TMP_Text timeText;

    void Update()
    {
        UpdateLighting();
    
        UpdateClockUI();
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

        RenderSettings.ambientLight = Color.Lerp(ambientNightColor, ambientDayColor, lightT);
    }

   

    private void UpdateClockUI()
    {
        if (timeText != null)
            timeText.text = FormatTime(timeOfDay);
    }

    private string FormatTime(float hour)
    {
        int h = Mathf.FloorToInt(hour);
        int m = Mathf.FloorToInt((hour - h) * 60f);
        return $"{h:00}:{m:00}";
    }
}
