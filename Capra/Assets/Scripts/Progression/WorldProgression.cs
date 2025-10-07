using TMPro;
using UnityEngine;
using System.Collections;
using System.Security;

public class WorldProgression : MonoBehaviour
{
    public static WorldProgression Instance { get; private set; }

    public GameObject TantiNuti1;
    public GameObject TantiNuti2;
    public GameObject TantiMariana1;
    public GameObject TantiMariana2;
   // public GameObject ItemReward;
    public TextMeshProUGUI ItemRewardText;

    [Header("CutScene Settings")]
    public GameObject CutSceneBackground;
    public TextMeshProUGUI WorldChangeCutScene; // this is a cutscene for when you do a good deed and the world changes
    public GameObject AudioWorldChange;
    public TextMeshProUGUI CutSceneStoryPart1; // this is the starts of the story and you get off a bus
    public GameObject AudioPart1;
    public GameObject CutSceneStoryPart2; // this is Capra's story
    public GameObject AudioPart2;
    public GameObject CutSceneStoryPart3; // tanti Didina's story
    public GameObject AudioPart3;
    public GameObject CutSceneStoryPart4; // Before party cutscene you hear that Tanti Geta passed away, but people come and help
    public GameObject AudioPart4;
    public GameObject CutSceneStoryPart5; // End Cutscene where capra comes back and leaves happy, knowing there will be a next year
    public GameObject AudioPart5;
    public float waitBeforeReward = 5f;
    public float waitBeforeCutscene = 5f;

    [Header("Player Settings")]
    public MonoBehaviour playerMovement; // drag your player movement script here
    public vThirdPersonCamera vCamera; // drag your camera object here
    public float introDuration = 5f;     // how long the intro lasts
    private bool introPlayed = false;
    private bool WorldChangedPlayed = false;
    private bool CapraStoryPlayed = false;
    public float fadeSpeed = 3f; // smaller = slower fade



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        PlayIntroCutscene();
    }

    // ==========================
    // INTRO CUTSCENE LOGIC
    // ==========================
    private void PlayIntroCutscene()
    {
        if (introPlayed) return;
        introPlayed = true;

        // Disable player movement during intro
        if (playerMovement != null)
            playerMovement.enabled = false;

        if (vCamera != null)
            vCamera.lockCamera = true; // camera locked, player can’t rotate

        // Show backrgound cutscene UI
        if (CutSceneBackground != null)
            CutSceneBackground.SetActive(true);

        // Show story cutscene UI
        if (CutSceneStoryPart1 != null)
            CutSceneStoryPart1.gameObject.SetActive(true);

        // Play intro audio if it exists
        if (AudioPart1 != null)
            AudioPart1.SetActive(true);

        // Automatically end after a few seconds
        // Invoke(nameof(EndIntroCutscene), introDuration);

        StartCoroutine(FadeTextRoutine());
        
    }

    private IEnumerator FadeTextRoutine()
    {
        Color c = CutSceneStoryPart1.color;
        c.a = 0;
        CutSceneStoryPart1.color = c;

        // Fade in
        while (c.a < 1f)
        {
            c.a += Time.deltaTime * fadeSpeed;
            CutSceneStoryPart1.color = c;
            yield return null;
        }

        // Wait while visible
        yield return new WaitForSeconds(introDuration - 2f);

        // Fade out
        while (c.a > 0f)
        {
            c.a -= Time.deltaTime * fadeSpeed;
            CutSceneStoryPart1.color = c;
            yield return null;
        }

        EndCutscene();
    }

    private void EndCutscene()
    {
        // Hide cutscene and audio

        // Show backrgound cutscene UI
        if (CutSceneBackground != null)
            CutSceneBackground.SetActive(false);

        if (CutSceneStoryPart1 != null)
            CutSceneStoryPart1.gameObject.SetActive(false);

        if (CutSceneStoryPart2 != null)
            CutSceneStoryPart2.gameObject.SetActive(false);

        if (AudioPart1 != null)
            AudioPart1.SetActive(false);

        if (WorldChangeCutScene != null)
            WorldChangeCutScene.gameObject.SetActive(false);

        // Enable player movement
        if (playerMovement != null)
            playerMovement.enabled = true;

        if (vCamera != null)
            vCamera.lockCamera = false; // restore normal camera control

    }

    // Apply world changes based on a reward code
    public void ApplyReward(string rewardCode)
    {
        string rewardMessage = "";
        bool shouldPlayCutscene = false;


        switch (rewardCode)
        {
            case "WoodCuttingFinished":
                TantiNuti1.SetActive(false);
                TantiNuti2.SetActive(true);
                rewardMessage = "Wood collected!";
                shouldPlayCutscene = false;
                break;

            case "FireWood":
                rewardMessage = "You received FireWood!";
                shouldPlayCutscene = true;
                break;
        
            case "EggCollectingFinished":
                TantiMariana1.SetActive(false);
                TantiMariana2.SetActive(true);
                rewardMessage = "Eggs collected!";
                shouldPlayCutscene = false;
                break;

            case "Pie":
                rewardMessage = "You received a Pie!";
                // Start delayed sequence for reward + cutscene
                //         StartCoroutine(RewardDelaySequence());
                shouldPlayCutscene = true;
                break;

        }

        if (!string.IsNullOrEmpty(rewardMessage))
        {
            if (shouldPlayCutscene)
                StartCoroutine(RewardDelaySequence(rewardMessage));
            else
            {
                // Just show the reward text without cutscene
                ItemRewardText.text = rewardMessage;
               // ItemReward.SetActive(true);

                // Optionally hide after some seconds
                StartCoroutine(HideRewardAfterDelay(3f));
            }
        }
    }
    private IEnumerator HideRewardAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
       // ItemReward.SetActive(false);
    }
    private IEnumerator RewardDelaySequence(string message)
    {
        // Wait before showing reward
        yield return new WaitForSeconds(waitBeforeReward);

        // Show the reward
        ItemRewardText.text = message;
        //ItemReward.SetActive(true);

        // Wait before world-change cutscene
        yield return new WaitForSeconds(waitBeforeCutscene);

        // Play world-change cutscene
        PlayWorldChangeCutScene();

       // ItemReward.SetActive(false);
    }


    private void PlayWorldChangeCutScene()
    {
        if (WorldChangedPlayed) return;
        WorldChangedPlayed = true;

        // Disable player movement during intro
        if (playerMovement != null)
            playerMovement.enabled = false;

        if (vCamera != null)
            vCamera.lockCamera = true; // camera locked, player can’t rotate


        // Show backrgound cutscene UI
        if (CutSceneBackground != null)
            CutSceneBackground.SetActive(true);

        // Show story cutscene UI
        if (WorldChangeCutScene != null)
            WorldChangeCutScene.gameObject.SetActive(true);

        // Play intro audio if it exists
        if (AudioWorldChange != null)
            AudioWorldChange.SetActive(true);

        // Automatically end after a few seconds
        // Invoke(nameof(EndIntroCutscene), introDuration);

        StartCoroutine(FadeTextRoutine());
    }

    public void PlayCapraCutscene()
    {
        if (CapraStoryPlayed) return;
        CapraStoryPlayed = true;

        // Disable player movement
        if (playerMovement != null)
            playerMovement.enabled = false;

        // Lock camera if using vThirdPersonCamera
        if (vCamera != null)
            vCamera.lockCamera = true;


        // Optional: Play audio if assigned
        if (AudioPart2 != null)
            AudioPart2.SetActive(true);

        // Show backrgound cutscene UI
        if (CutSceneBackground != null)
            CutSceneBackground.SetActive(true);

        // Show story cutscene UI
        if (CutSceneStoryPart2 != null)
            CutSceneStoryPart2.gameObject.SetActive(true);


        StartCoroutine(FadeTextRoutine());
    }


}
