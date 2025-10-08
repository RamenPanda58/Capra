using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Cycle Durations (seconds)")]
    public float dayDuration = 240f;   // length of the day in seconds
    public float nightDuration = 240f; // length of the night in seconds

    [Header("Sun Settings")]
    public Transform sunPivot;          // Empty object at the world center
    public Light directionalLight;      // Your Sun/Moon
    public Color dayColor = new Color(1f, 0.956f, 0.839f);
    public Color nightColor = new Color(0.2f, 0.3f, 0.5f);
    public float sunTilt = 23.5f;      // axial tilt for realism

    [Header("Ambient Light Settings")]
    public Color ambientDayColor = Color.white;
    public Color ambientNightColor = new Color(0.1f, 0.1f, 0.2f);

    [Header("Start Time Settings")]
    [Range(0f, 24f)] public float startHour = 17f; // start at 5 PM by default

    private bool isDay;
    private float timer;
    private float currentPhaseDuration;

    void Start()
    {
        // Determine if we start in day or night
        if (startHour >= 6f && startHour < 18f)
        {
            isDay = true;
            currentPhaseDuration = dayDuration;
            timer = ((startHour - 6f) / 12f) * dayDuration;
        }
        else
        {
            isDay = false;
            currentPhaseDuration = nightDuration;
            float nightStart = 18f;
            timer = ((startHour - nightStart + 24f) % 24f / 12f) * nightDuration;
        }

        UpdateLightingInstant();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentPhaseDuration)
        {
            if (isDay) StartNight();
            else StartDay();
        }

        // Smoothly rotate sun around pivot
        float t = Mathf.Clamp01(timer / currentPhaseDuration);
        float sunRotation = 0f;

        if (isDay)
            sunRotation = Mathf.Lerp(-90f, 90f, t); // from sunrise (-90°) to sunset (90°)
        else
            sunRotation = Mathf.Lerp(90f, 270f, t); // from sunset to next sunrise

        sunPivot.localRotation = Quaternion.Euler(sunRotation, 0f, sunTilt);

        // Smoothly update colors
        float colorT = t;
        if (!isDay) colorT = 1f - t;

        directionalLight.color = Color.Lerp(nightColor, dayColor, colorT);
        RenderSettings.ambientLight = Color.Lerp(ambientNightColor, ambientDayColor, colorT);
    }

    private void StartDay()
    {
        isDay = true;
        currentPhaseDuration = dayDuration;
        timer = 0f;
    }

    private void StartNight()
    {
        isDay = false;
        currentPhaseDuration = nightDuration;
        timer = 0f;
    }

    // Instantly set light to current time
    private void UpdateLightingInstant()
    {
        float t = Mathf.Clamp01(timer / currentPhaseDuration);
        float sunRotation = isDay ? Mathf.Lerp(-90f, 90f, t) : Mathf.Lerp(90f, 270f, t);
        sunPivot.localRotation = Quaternion.Euler(sunRotation, 0f, sunTilt);

        float colorT = isDay ? t : 1f - t;
        directionalLight.color = Color.Lerp(nightColor, dayColor, colorT);
        RenderSettings.ambientLight = Color.Lerp(ambientNightColor, ambientDayColor, colorT);
    }

    // Optional debug keys
    void UpdateDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.D)) StartDay();
        if (Input.GetKeyDown(KeyCode.N)) StartNight();
    }

    public void ForceDay() => StartDay();
    public void ForceNight() => StartNight();
}
