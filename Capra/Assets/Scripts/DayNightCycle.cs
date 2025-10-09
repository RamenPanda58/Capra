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
    public float sunTilt = 23.5f;       // axial tilt for realism

    [Header("Ambient Light Settings")]
    public Color ambientDayColor = Color.white;
    public Color ambientNightColor = new Color(0.1f, 0.1f, 0.2f);

    [Header("Start Time Settings")]
    [Range(0f, 24f)] public float startHour = 17f; // start at 5 PM by default

    [Header("Day Counter")]
    public int dayCount = 0; // Start at 0 so first full day is "Day 1"

    [Header("Dialogue UI")]
    public GameObject dialogueUI; // Turned on/off depending on day/night

    private bool isDay;
    private float timer;
    private float currentPhaseDuration;

    public System.Action OnNewDay;

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
        UpdateDialogueState();

        Debug.Log($"Game started at {FormatTime(startHour)} — Night will fall soon.");
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentPhaseDuration)
        {
            if (isDay) StartNight();
            else StartDay();
        }

        float t = Mathf.Clamp01(timer / currentPhaseDuration);
        float sunRotation = 0f;

        if (isDay)
            sunRotation = Mathf.Lerp(-90f, 90f, t); // sunrise to sunset
        else
            sunRotation = Mathf.Lerp(90f, 270f, t); // sunset to next sunrise

        sunPivot.localRotation = Quaternion.Euler(sunRotation, 0f, sunTilt);

        float colorT = t;
        if (!isDay) colorT = 1f - t;

        directionalLight.color = Color.Lerp(nightColor, dayColor, colorT);
        RenderSettings.ambientLight = Color.Lerp(ambientNightColor, ambientDayColor, colorT);

        float currentHour = CalculateCurrentHour();
        if (Mathf.Abs(Time.frameCount % 60) < 0.1f)
        {
            string phase = isDay ? "Day" : "Night";
            Debug.Log($"[{phase}] Current Time: {FormatTime(currentHour)}  |  Day: {dayCount}");
        }

        UpdateDebugKeys();
    }

    private void StartDay()
    {
        isDay = true;
        currentPhaseDuration = dayDuration;
        timer = 0f;

        if (dayCount > 0 || startHour < 6f || startHour >= 18f)
        {
            dayCount++;
            Debug.Log($"A new day has started. Day: {dayCount}");
            OnNewDay?.Invoke();
        }
        else
        {
            dayCount = 1;
            Debug.Log("First full day begins.");
            OnNewDay?.Invoke();
        }

        Debug.Log("It is now DAYTIME.");
        UpdateDialogueState();
    }

    private void StartNight()
    {
        isDay = false;
        currentPhaseDuration = nightDuration;
        timer = 0f;
        Debug.Log("It is now NIGHTTIME.");
        UpdateDialogueState();
    }

    private void UpdateDialogueState()
    {
        if (dialogueUI != null)
        {
            dialogueUI.SetActive(isDay);
        }
    }

    private float CalculateCurrentHour()
    {
        if (isDay)
        {
            float t = Mathf.Clamp01(timer / dayDuration);
            return Mathf.Lerp(6f, 18f, t);
        }
        else
        {
            float t = Mathf.Clamp01(timer / nightDuration);
            float hour = Mathf.Lerp(18f, 30f, t);
            if (hour >= 24f) hour -= 24f;
            return hour;
        }
    }

    private string FormatTime(float hour)
    {
        int h = Mathf.FloorToInt(hour);
        int m = Mathf.FloorToInt((hour - h) * 60f);
        return $"{h:00}:{m:00}";
    }

    private void UpdateLightingInstant()
    {
        float t = Mathf.Clamp01(timer / currentPhaseDuration);
        float sunRotation = isDay ? Mathf.Lerp(-90f, 90f, t) : Mathf.Lerp(90f, 270f, t);
        sunPivot.localRotation = Quaternion.Euler(sunRotation, 0f, sunTilt);

        float colorT = isDay ? t : 1f - t;
        directionalLight.color = Color.Lerp(nightColor, dayColor, colorT);
        RenderSettings.ambientLight = Color.Lerp(ambientNightColor, ambientDayColor, colorT);
    }

    void UpdateDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.D)) StartDay();
        if (Input.GetKeyDown(KeyCode.N)) StartNight();
    }

    public void ForceDay() => StartDay();
    public void ForceNight() => StartNight();

    public bool IsDaytime() => isDay;
}
