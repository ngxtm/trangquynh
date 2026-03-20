using System;
using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private UnityEngine.UI.Text tutorialText;
    [SerializeField] private GameObject highlightOverlay;
    [SerializeField] private UnityEngine.UI.Button nextButton;
    [SerializeField] private UnityEngine.UI.Button skipButton;

    private int currentStep = 0;
    private bool isActive = false;
    private Action onComplete;

    private readonly string[] tutorialSteps = new string[]
    {
        "Chào mừng đến với Ô Ăn Quan!\nĐây là trò chơi dân gian Việt Nam.",
        "Mục tiêu: Ăn được nhiều quân hơn đối thủ.\nQuân lớn (10 điểm) và dân (1 điểm).",
        "Chọn một ô bên phía của bạn (5 ô dưới) để bắt đầu.",
        "Chọn hướng rải quân: Trái hoặc Phải.",
        "Khi rải hết vào ô trống, ô tiếp theo có quân sẽ bị ăn!\nChúc bạn chơi vui!",
    };

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
        if (tutorialPanel) tutorialPanel.SetActive(false);
        if (highlightOverlay) highlightOverlay.SetActive(false);
        
        if (nextButton) nextButton.onClick.AddListener(NextStep);
        if (skipButton) skipButton.onClick.AddListener(SkipTutorial);

        if (!PlayerPrefs.HasKey("TutorialCompleted"))
        {
            StartCoroutine(DelayedStart());
        }
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(1f);
        StartTutorial(null);
    }

    public void StartTutorial(Action onCompleteCallback)
    {
        if (isActive) return;

        currentStep = 0;
        isActive = true;
        onComplete = onCompleteCallback;

        if (tutorialPanel) tutorialPanel.SetActive(true);
        ShowStep(currentStep);
    }

    private void ShowStep(int step)
    {
        if (step >= tutorialSteps.Length)
        {
            CompleteTutorial();
            return;
        }

        if (tutorialText) tutorialText.text = tutorialSteps[step];

        switch (step)
        {
            case 2:
                HighlightPlayerCells();
                break;
            case 3:
                HighlightArrows();
                break;
            default:
                HideHighlight();
                break;
        }

        SoundManager.Instance?.PlaySFX(Config.SFX.CLICK);
    }

    private void NextStep()
    {
        currentStep++;
        ShowStep(currentStep);
    }

    private void SkipTutorial()
    {
        CompleteTutorial();
    }

    private void CompleteTutorial()
    {
        isActive = false;
        if (tutorialPanel) tutorialPanel.SetActive(false);
        HideHighlight();

        PlayerPrefs.SetInt("TutorialCompleted", 1);
        PlayerPrefs.Save();

        onComplete?.Invoke();
        SoundManager.Instance?.PlaySFX(Config.SFX.START_GAME);
    }

    private void HighlightPlayerCells()
    {
        if (highlightOverlay) highlightOverlay.SetActive(true);
    }

    private void HighlightArrows()
    {
        if (highlightOverlay) highlightOverlay.SetActive(true);
    }

    private void HideHighlight()
    {
        if (highlightOverlay) highlightOverlay.SetActive(false);
    }

    public void ResetTutorial()
    {
        PlayerPrefs.DeleteKey("TutorialCompleted");
        PlayerPrefs.Save();
    }

    public bool IsActive() => isActive;
}
