using UnityEngine;
using TMPro;
using System.Collections;

public class DayNightCycle : MonoBehaviour
{
    [Header("Cycle Durations (seconds)")]
    public float dayDuration = 240f;
    public float nightDuration = 240f;

    [Header("Sun Settings")]
    public Transform sunPivot;
    public Light directionalLight;
    public Color dayColor = new Color(1f, 0.956f, 0.839f);
    public Color nightColor = new Color(0.2f, 0.3f, 0.5f);
    public float sunTilt = 23.5f;

    [Header("Ambient Light Settings")]
    public Color ambientDayColor = Color.white;
    public Color ambientNightColor = new Color(0.1f, 0.1f, 0.2f);

    [Header("Start Time Settings")]
    [Range(0f, 24f)] public float startHour = 17f;

    [Header("Day Counter")]
    public int dayCount = 0;

    [Header("Dialogue UI")]
    public GameObject dialogueUI;

    [Header("Environment GameObjects")]
    public GameObject dayBirdsObject;
    public GameObject nightBirdsObject;
    public GameObject villagersObject;

    [Header("Morning Message UI")]
    public GameObject morningMessageBackground;
    public TMP_Text morningMessageText;
    public float morningMessageDuration = 3f;

    private bool isDay;
    private float timer;
    private float currentPhaseDuration;
    private bool hasShownMorningMessage = false;

    public System.Action OnNewDay;

    private const float dayStartHour = 5f;
    private const float nightStartHour = 20f;

    private Coroutine morningMessageRoutine;
    private CanvasGroup backgroundCanvasGroup;

    void Start()
    {
        if (morningMessageBackground != null)
            backgroundCanvasGroup = morningMessageBackground.GetComponent<CanvasGroup>();

        if (startHour >= dayStartHour && startHour < nightStartHour)
        {
            isDay = true;
            currentPhaseDuration = dayDuration;
            timer = ((startHour - dayStartHour) / (nightStartHour - dayStartHour)) * dayDuration;
        }
        else
        {
            isDay = false;
            currentPhaseDuration = nightDuration;
            float adjustedHour = (startHour >= nightStartHour) ? startHour - nightStartHour : startHour + (24f - nightStartHour);
            timer = (adjustedHour / ((24f - nightStartHour) + dayStartHour)) * nightDuration;
        }

        UpdateLightingInstant();
        UpdateEnvironmentObjects();
        HideMorningMessageInstant();
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
        float sunRotation = isDay ? Mathf.Lerp(-90f, 90f, t) : Mathf.Lerp(90f, 270f, t);
        sunPivot.localRotation = Quaternion.Euler(sunRotation, 0f, sunTilt);

        float colorT = isDay ? t : 1f - t;
        directionalLight.color = Color.Lerp(nightColor, dayColor, colorT);
        RenderSettings.ambientLight = Color.Lerp(ambientNightColor, ambientDayColor, colorT);

        float currentHour = CalculateCurrentHour();

        if (isDay)
        {
            if (!hasShownMorningMessage && currentHour >= 6.5f && currentHour < 7f)
            {
                hasShownMorningMessage = true;
                ShowMorningMessage("It's day " + dayCount + "! Let's make the most out of it!");
            }
        }

        //  Villager activation condition
        if (villagersObject != null)
        {
            bool shouldBeActive = isDay && currentHour >= 6.5f && currentHour < nightStartHour;
            if (villagersObject.activeSelf != shouldBeActive)
                villagersObject.SetActive(shouldBeActive);
        }

        if (Time.frameCount % 60 == 0)
        {
            string phase = isDay ? "Day" : "Night";
            Debug.Log("[" + phase + "] Current Time: " + FormatTime(currentHour) + " | Day: " + dayCount);
        }
    }

    private string FormatTime(float hour)
    {
        int h = Mathf.FloorToInt(hour);
        int m = Mathf.FloorToInt((hour - h) * 60f);
        return h.ToString("00") + ":" + m.ToString("00");
    }

    private void StartDay()
    {
        isDay = true;
        currentPhaseDuration = dayDuration;
        timer = 0f;

        if (dayCount > 0 || startHour < dayStartHour || startHour >= nightStartHour)
        {
            dayCount++;
            OnNewDay?.Invoke();
        }
        else
        {
            dayCount = 1;
            OnNewDay?.Invoke();
        }

        hasShownMorningMessage = false;
        UpdateEnvironmentObjects();
    }

    private void StartNight()
    {
        isDay = false;
        currentPhaseDuration = nightDuration;
        timer = 0f;
        UpdateEnvironmentObjects();
    }

    private void UpdateEnvironmentObjects()
    {
        if (dialogueUI != null)
            dialogueUI.SetActive(isDay);

        if (dayBirdsObject != null)
            dayBirdsObject.SetActive(isDay);
        if (nightBirdsObject != null)
            nightBirdsObject.SetActive(!isDay);

        // villagers handled dynamically in Update()
    }

    private float CalculateCurrentHour()
    {
        if (isDay)
        {
            float t = Mathf.Clamp01(timer / dayDuration);
            return Mathf.Lerp(dayStartHour, nightStartHour, t);
        }
        else
        {
            float t = Mathf.Clamp01(timer / nightDuration);
            float hour = Mathf.Lerp(nightStartHour, 29f, t);
            if (hour >= 24f) hour -= 24f;
            return hour;
        }
    }

    private void ShowMorningMessage(string message)
    {
        if (morningMessageRoutine != null)
            StopCoroutine(morningMessageRoutine);

        morningMessageRoutine = StartCoroutine(MorningMessageRoutine(message));
    }

    private IEnumerator MorningMessageRoutine(string message)
    {
        if (morningMessageBackground == null || morningMessageText == null)
            yield break;

        if (backgroundCanvasGroup == null)
            backgroundCanvasGroup = morningMessageBackground.GetComponent<CanvasGroup>();

        morningMessageBackground.SetActive(true);
        morningMessageText.text = message;

        yield return StartCoroutine(FadeCanvasGroup(backgroundCanvasGroup, 0f, 1f, 0.5f));

        yield return new WaitForSeconds(morningMessageDuration);

        yield return StartCoroutine(FadeCanvasGroup(backgroundCanvasGroup, 1f, 0f, 0.5f));

        morningMessageBackground.SetActive(false);
        morningMessageText.text = "";
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            cg.alpha = Mathf.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        cg.alpha = end;
    }

    private void HideMorningMessageInstant()
    {
        if (backgroundCanvasGroup != null)
            backgroundCanvasGroup.alpha = 0f;
        if (morningMessageBackground != null)
            morningMessageBackground.SetActive(false);
        if (morningMessageText != null)
            morningMessageText.text = "";
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

    public void ForceDay() => StartDay();
    public void ForceNight() => StartNight();
    public bool IsDaytime() => isDay;
}
