using TMPro;
using UnityEngine;
using System.Collections;
using System.Security;

public class WorldProgression : MonoBehaviour
{
    public static WorldProgression Instance { get; private set; }

    [Header("World Change Progression")]
    [SerializeField] private int worldChangeCounter = 0; // starts at 0
    [SerializeField] private int maxWorldChanges = 3;
    [SerializeField] private float delayBeforeWorldCutscene = 2f;

    [Header("Villagers")]
    public GameObject TantiNuti1;
    public GameObject TantiNuti2;
    public GameObject TantiNuti3;
    public GameObject TantiMariana1;
    public GameObject TantiMariana2;
    public GameObject TantiMariana3;
    public GameObject NeneaMarian1;
    public GameObject NeneaMarian2;
    public GameObject NeneaMarian3;
    public GameObject Capra1;
    public GameObject Capra2;
    // public GameObject ItemReward;
    public TextMeshProUGUI ItemRewardText;
    [SerializeField] private GameObject TantiGetaWaiting;

    [Header("CutScene Settings")]
    public GameObject CutSceneBackground;
    public TextMeshProUGUI WorldChangeCutScene; // this is a cutscene for when you do a good deed and the world changes
    public TextMeshProUGUI WorldChangeCutScene2;
    public TextMeshProUGUI WorldChangeCutScene3;
    public GameObject AudioWorldChange;
    public TextMeshProUGUI CutSceneStoryPart1; // this is the starts of the story and you get off a bus
    public GameObject AudioPart1;
    public GameObject CutSceneStoryPart2; // this is Capra's story
    public GameObject AudioPart2;
    public GameObject TantiDidinaCutScene; // tanti Didina's story
    public GameObject AudioPart3;
    public GameObject CutSceneStoryPart4; // Before party cutscene you hear that Tanti Geta passed away, but people come and help
    public GameObject AudioPart4;
    public GameObject CutSceneStoryPart5; // End Cutscene where capra comes back and leaves happy, knowing there will be a next year
    public GameObject AudioPart5;
    public GameObject NightBeforeCelebrationCutScene;
    public float waitBeforeReward = 5f;
    public float waitBeforeCutscene = 5f;
    public GameObject CelebrationNoteUI;
    public GameObject WorldChangePp1;
    public GameObject WorldChangePp2;
    public GameObject WorldChangePp3;
    public GameObject SomberPp;
    public GameObject Snow; 

    [Header("Player Settings")]
    public MonoBehaviour playerMovement; // drag your player movement script here
    public vThirdPersonCamera vCamera; // drag your camera object here
    private bool introPlayed = false;
    private bool WorldChangedPlayed = false;
    private bool CapraStoryPlayed = false;
    public float fadeSpeed = 3f; // smaller = slower fade

    private bool dialogueJustEnded = false;
    private bool fireWoodReceived = false;
    private bool pieReceived = false;
    private bool icoanaReceived = false;



    [Header("Character Interaction Settings")]
    [SerializeField] private GameObject TantiGeta1; // initial model
    [SerializeField] private GameObject TantiGeta2; // second model
    [SerializeField] private GameObject TantiGeta3; // third model
   // [SerializeField] private GameObject TantiGeta4; // fourth model
   // [SerializeField] private int interactionsRequired = 3; // number of talks before triggering
    [SerializeField] private GameObject TantiDidinaLocation; // the location where the cutscene should play
    private bool tantiDidinaReady = false;
    public GameObject Ielele;
    [SerializeField] private float cutsceneDelay = 1f; // optional delay before cutscene

   // private int currentInteractionCount = 0;
    private bool cutsceneTriggered = false;

    private Coroutine hideRewardCoroutine;

    private bool canGetTantiGetaReward = true; // only for TantiGeta
    private float tantiGetaCooldown = 20f;     // 10 seconds cooldown


    [Header("Final Cutscene Settings")]
    [SerializeField] private GameObject FinalCutsceneTriggerLocation; // location collider the player can enter
    private bool finalCutsceneReady = false;
    private bool finalCutscenePlayed = false;

    private bool woodCollected = false;
    private bool eggsCollected = false;
    private bool weedsCollected = false;

    public GameObject CelebrationLighting;
    public GameObject GlobalVolumeSomber;
    public GameObject GlobalVolumeCelebration;

    [Header("Cutscene Durations")]
    public float introDuration = 5f;     // how long the intro lasts
    public float capraDuration = 7f;
    public float tantiDidinaDuration = 6f;
    public float finalCutsceneDuration = 8f;
    public float worldChangeDuration = 6f;



    [Header("UI Settings")]
    [SerializeField] private float rewardTextDuration = 3f; // how long the reward text stays on screen

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

    private void Update()
    {
        // ===== DEBUG KEYS FOR TESTING REWARDS AND CUTSCENES =====
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("[DEBUG] Triggering WoodCuttingFinished reward manually.");
            ApplyReward("WoodCuttingFinished");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("[DEBUG] Triggering EggCollectingFinished reward manually.");
            ApplyReward("EggCollectingFinished");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("[DEBUG] Triggering WeedingFinished reward manually.");
            ApplyReward("WeedingFinished");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("[DEBUG] Triggering Basma reward manually (Tanti Didina unlock).");
            ApplyReward("Basma");
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("[DEBUG] Triggering Final Cutscene manually.");
            TriggerFinalCutscene();
        }

        // DEBUG: Skip Intro Cutscene
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("[DEBUG] Skipping intro cutscene...");
            StopAllCoroutines(); // stop any ongoing fade
            EndCutscene();       // cleanly end it
            introPlayed = true;  // mark it as played so it won’t replay
        }

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

            StartCoroutine(FadeTextRoutine(CutSceneStoryPart1, introDuration));

    }

    private IEnumerator FadeTextRoutine(Object element, float duration)
    {
        // Handle TextMeshProUGUI type
        if (element is TextMeshProUGUI textElement)
        {
            Color c = textElement.color;
            c.a = 0;
            textElement.color = c;
            textElement.gameObject.SetActive(true);

            // Fade in
            while (c.a < 1f)
            {
                c.a += Time.deltaTime * fadeSpeed;
                textElement.color = c;
                yield return null;
            }

            // Wait while visible
            yield return new WaitForSeconds(duration - 2f);

            // Fade out
            while (c.a > 0f)
            {
                c.a -= Time.deltaTime * fadeSpeed;
                textElement.color = c;
                yield return null;
            }

            c.a = 0f;
            textElement.color = c;
            textElement.gameObject.SetActive(false);
        }
        // Handle GameObject type
        else if (element is GameObject go)
        {
            go.SetActive(true);
            yield return new WaitForSeconds(duration);
            go.SetActive(false);
        }
        else
        {
            Debug.LogWarning($"[FadeTextRoutine] Unsupported element type: {element}");
        }

        // Call universal end logic
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

        if (TantiDidinaCutScene != null)
            TantiDidinaCutScene.gameObject.SetActive(false);


        if (CutSceneStoryPart4 != null)
            CutSceneStoryPart4.gameObject.SetActive(false);

        Ielele.SetActive(false);

        if (AudioPart1 != null)
            AudioPart1.SetActive(false);

        if (AudioPart2 != null)
            AudioPart2.SetActive(false);

        if (AudioPart3 != null)
            AudioPart3.SetActive(false);

        if (AudioWorldChange != null)
            AudioWorldChange.SetActive(false);

        if (WorldChangeCutScene != null)
            WorldChangeCutScene.gameObject.SetActive(false);

        if (WorldChangeCutScene2 != null)
            WorldChangeCutScene2.gameObject.SetActive(false);

        if (WorldChangeCutScene3 != null)
            WorldChangeCutScene3.gameObject.SetActive(false);

        if (AudioPart4 != null)
            AudioPart4.SetActive(false);

        // Enable player movement
        if (playerMovement != null)
            playerMovement.enabled = true;

        if (vCamera != null)
            vCamera.lockCamera = false; // restore normal camera control

    }

    public void OnDialogueEndedForCurrentTask()
    {
        Debug.Log("[WorldProgression] OnDialogueEndedForCurrentTask() was called!");

        if (TaskManager.Instance.currentTask != null)
        {
            string rewardCode = TaskManager.Instance.currentTask.RewardCode;
            Debug.Log($"Dialogue ended. Applying reward for current task: {rewardCode}");

            dialogueJustEnded = true;

            ApplyReward(rewardCode);

        }
        else
        {
            Debug.LogWarning("Dialogue ended, but no current task found!");
        }
    }

    private bool CheckTantiGetaCooldown(string rewardCode)
    {
        if (rewardCode == "Cozonac" || rewardCode == "DriedPlants" || rewardCode == "Basma")
        {
            if (!canGetTantiGetaReward)
            {
                ShowRewardMessage("Come back later!");
                dialogueJustEnded = false;  // prevent dialogue from progressing
                return false;
            }
        }

        return true;
    }



    // Apply world changes based on a reward code
    public void ApplyReward(string rewardCode)
    {
        // ----- CHECK COOLDOWN FIRST -----
        if (!CheckTantiGetaCooldown(rewardCode))
            return; // stop everything if on cooldown
                    // --------------------------------

        string rewardMessage = "";
        bool shouldPlayCutscene = false;
        bool shouldWaitForDialogue = false;

        ResetOpacity();
        switch (rewardCode)
        {
            case "WoodCuttingFinished":
                TantiNuti1.SetActive(false);
                TantiNuti2.SetActive(true);
                rewardMessage = "Wood collected!";
                shouldPlayCutscene = false;
                ResetOpacity();
                woodCollected = true;
                break;

            case "FireWood":
                rewardMessage = "You received FireWood!";
                //  shouldPlayCutscene = true;
                IncrementWorldChange();
                shouldWaitForDialogue = true;
                ResetOpacity();
                fireWoodReceived = true;
                break;

            case "EggCollectingFinished":
                TantiMariana1.SetActive(false);
                TantiMariana2.SetActive(true);
                rewardMessage = "Eggs collected!";
                shouldPlayCutscene = false;
                ResetOpacity();
                eggsCollected = true;
                break;

            case "Pie":
                rewardMessage = "You received a Pie!";
                // Start delayed sequence for reward + cutscene
                //         StartCoroutine(RewardDelaySequence());
                // shouldPlayCutscene = true;
                IncrementWorldChange();
                shouldWaitForDialogue = true;
                ResetOpacity();
                pieReceived = true;
                break;

            case "WeedingFinished":
                NeneaMarian1.SetActive(false);
                NeneaMarian2.SetActive(true);
                rewardMessage = "Weeds collected!";
                shouldPlayCutscene = false;
                ResetOpacity();
                weedsCollected = true;
                break;

            case "Icoana":
                rewardMessage = "You received an Icoana!";
                //  shouldPlayCutscene = true;
                IncrementWorldChange();
                shouldWaitForDialogue = true;
                ResetOpacity();
                icoanaReceived = true;
                break;

            case "Cozonac":
                // Show the reward message immediately
                ShowRewardMessage("You received a Cozonac!");

                // Activate waiting visual
                if (TantiGetaWaiting != null)
                    TantiGetaWaiting.SetActive(true);

                // Start cooldown / delayed transform
                StartCoroutine(TantiGetaDelayedTransform(rewardCode));
                break;

            case "DriedPlants":
                ShowRewardMessage("You received dried plants!");

                if (TantiGetaWaiting != null)
                    TantiGetaWaiting.SetActive(true);

                StartCoroutine(TantiGetaDelayedTransform(rewardCode));
                break;

            case "Basma":
                ShowRewardMessage("You received a basma!");

                if (TantiGetaWaiting != null)
                    TantiGetaWaiting.SetActive(true);

                StartCoroutine(TantiGetaDelayedTransform(rewardCode));
                break;
                // Start cooldown / transformation coroutine
               
             

        }


        CheckFinalCutsceneCondition();

        if (shouldWaitForDialogue && !dialogueJustEnded)
        {
            Debug.Log("Dialogue not finished yet — skipping reward display for now.");
            return;
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
                StartCoroutine(HideRewardAfterDelay(rewardTextDuration));
            }
        }

        //  reset flag after we’re sure everything started
        if (shouldWaitForDialogue)
            dialogueJustEnded = false;

    }

    private IEnumerator TantiGetaDelayedTransform(string rewardCode)
    {
        canGetTantiGetaReward = false;
        yield return new WaitForSeconds(tantiGetaCooldown);

        // Hide waiting visual
        if (TantiGetaWaiting != null)
            TantiGetaWaiting.SetActive(false);

        // Transform the TantiGeta model or unlock Basma
        switch (rewardCode)
        {
            case "Cozonac":
                TantiGeta1.SetActive(false);
                TantiGeta2.SetActive(true);
                break;

            case "DriedPlants":
                TantiGeta2.SetActive(false);
                TantiGeta3.SetActive(true);
                break;

            case "Basma":
                tantiDidinaReady = true;
                Ielele.SetActive(true);
                if (TantiDidinaLocation != null)
                    TantiDidinaLocation.SetActive(true);
                break;
        }

        canGetTantiGetaReward = true;
    }


    private void CheckFinalCutsceneCondition()
    {
        if (woodCollected && eggsCollected && weedsCollected
            && fireWoodReceived && pieReceived && icoanaReceived
            && !finalCutsceneReady)
        {
            finalCutsceneReady = true;
            Debug.Log("All world tasks AND rewards completed! Final cutscene location unlocked.");

            if (FinalCutsceneTriggerLocation != null)
            {
                FinalCutsceneTriggerLocation.SetActive(true); // activate the collider
                Debug.Log("FinalCutsceneTriggerLocation is now active.");

                TantiGeta1.SetActive(false);
                TantiGeta2.SetActive(false);
                TantiGeta3.SetActive(false);
                TantiMariana2.SetActive(false);
                TantiNuti2.SetActive(false);
                NeneaMarian2.SetActive(false);
                TantiMariana3.SetActive(true);
                TantiNuti3.SetActive(true);
                NeneaMarian3.SetActive(true);

                if (CelebrationNoteUI != null)
                {
                    CelebrationNoteUI.SetActive(true);
                    Debug.Log("Celebration Note UI is now active to guide the player.");
                }
            }
        }
    }


    private IEnumerator TantiGetaCooldown()
    {
        canGetTantiGetaReward = false;
        yield return new WaitForSeconds(tantiGetaCooldown);
        canGetTantiGetaReward = true;
    }

    private void ResetOpacity()
    {
        Color c = ItemRewardText.color;
        c.a = 1.0f;
        ItemRewardText.color = c;
    }

    private void IncrementWorldChange()
    {
        if (worldChangeCounter >= maxWorldChanges)
            return;

        worldChangeCounter++;
        Debug.Log($"World Change Count increased to: {worldChangeCounter}");

        StartCoroutine(PlayWorldChangeCutScene(worldChangeCounter));

    }

    private void ShowRewardMessage(string message)
    {
        // Set the UI text
        ItemRewardText.text = message;

        // Make sure it’s visible
        Color c = ItemRewardText.color;
        c.a = 1f;
        ItemRewardText.color = c;

        // Stop previous hide coroutine if running
        if (hideRewardCoroutine != null)
            StopCoroutine(hideRewardCoroutine);

        // Start fade/hide after delay
        hideRewardCoroutine = StartCoroutine(HideRewardAfterDelay(rewardTextDuration));
    }
    private IEnumerator HideRewardAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        float fadeDuration = 1f;
        Color c = ItemRewardText.color;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            c.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            ItemRewardText.color = c;
            yield return null;
        }

        c.a = 0f;
        ItemRewardText.color = c;
        ItemRewardText.text = "";
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
        StartCoroutine(PlayWorldChangeCutScene(worldChangeCounter));


        // ItemReward.SetActive(false);
    }



    private IEnumerator PlayWorldChangeCutScene(int stage)
    {
        yield return new WaitForSeconds(delayBeforeWorldCutscene);

        // Disable player movement during cutscene
        if (playerMovement != null)
            playerMovement.enabled = false;

        if (vCamera != null)
            vCamera.lockCamera = true;

        // Show background
        if (CutSceneBackground != null)
            CutSceneBackground.SetActive(true);

        switch (stage)
        {
            case 1:
                if (WorldChangeCutScene != null)
                    WorldChangeCutScene.gameObject.SetActive(true);
                if (AudioWorldChange != null)
                    AudioWorldChange.SetActive(true);
                if (SomberPp != null)
                    SomberPp.gameObject.SetActive(false);
                if (WorldChangePp1 != null)
                    WorldChangePp1.gameObject.SetActive(true);
                StartCoroutine(FadeTextRoutine(WorldChangeCutScene, worldChangeDuration));

                break;

            case 2:
                if (WorldChangeCutScene2 != null)
                    WorldChangeCutScene2.gameObject.SetActive(true);
                if (AudioWorldChange != null)
                    AudioWorldChange.SetActive(true);
                if (WorldChangePp1 != null)
                    WorldChangePp1.gameObject.SetActive(false);
                if (WorldChangePp2 != null)
                    WorldChangePp2.gameObject.SetActive(true);
                if (Snow != null)
                    Snow.gameObject.SetActive(false);
                StartCoroutine(FadeTextRoutine(WorldChangeCutScene2, worldChangeDuration));
         
                break;

            case 3:
                if (WorldChangeCutScene3 != null)
                    WorldChangeCutScene3.gameObject.SetActive(true);
                if (AudioWorldChange != null)
                    AudioWorldChange.SetActive(true);
                if (WorldChangePp2 != null)
                    WorldChangePp2.gameObject.SetActive(false);
                if (WorldChangePp3 != null)
                    WorldChangePp3.gameObject.SetActive(true);
                StartCoroutine(FadeTextRoutine(WorldChangeCutScene3, worldChangeDuration));
                
                break;
        }

        //StartCoroutine(FadeTextRoutine());
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


        StartCoroutine(FadeTextRoutine(CutSceneStoryPart2, capraDuration));
    }

    public void TriggerTantiDidinaCutscene()
    {
        if (!tantiDidinaReady) return; // Only trigger if player got the Basma
        if (TantiDidinaCutScene == null) return;

        Debug.Log("Tanti Didina cutscene triggered!");

        // Disable player movement
        if (playerMovement != null)
            playerMovement.enabled = false;

        if (vCamera != null)
            vCamera.lockCamera = true;

        // Activate cutscene visuals
        if (CutSceneBackground != null)
            CutSceneBackground.SetActive(true);

        TantiDidinaCutScene.SetActive(true);

        if (AudioPart3 != null)
            AudioPart3.SetActive(true);

        // Optional: use your fade routine
        StartCoroutine(FadeTextRoutine(TantiDidinaCutScene, tantiDidinaDuration));

        // Prevent retriggering
        tantiDidinaReady = false;

        // Disable location so it doesn’t trigger again
        if (TantiDidinaLocation != null)
            TantiDidinaLocation.SetActive(false);

        StartCoroutine(FadeTextRoutine(TantiDidinaCutScene, tantiDidinaDuration));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!finalCutsceneReady || finalCutscenePlayed)
            return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the final cutscene area.");
            TriggerFinalCutscene();
        }
    }

    public void TriggerFinalCutscene()
    {
        finalCutscenePlayed = true;

        // Disable player movement
        if (playerMovement != null)
            playerMovement.enabled = false;

        if (vCamera != null)
            vCamera.lockCamera = true;

        // Activate visuals and audio
        if (CutSceneBackground != null)
            CutSceneBackground.SetActive(true);

        if (CutSceneStoryPart4 != null)
            CutSceneStoryPart4.SetActive(true);

        if (AudioPart4 != null)
            AudioPart4.SetActive(true);

        StartCoroutine(FadeTextRoutine(CutSceneStoryPart4, finalCutsceneDuration));

        // Optionally disable collider so it doesn’t trigger again
        if (FinalCutsceneTriggerLocation != null)
            FinalCutsceneTriggerLocation.SetActive(false);

        Capra2.SetActive(true);
        Capra1.SetActive(false);
        CelebrationLighting.SetActive(true);
        GlobalVolumeSomber.SetActive(false);
        GlobalVolumeCelebration.SetActive(true);

        StartCoroutine(FadeTextRoutine(CutSceneStoryPart4, finalCutsceneDuration));
    }


}
