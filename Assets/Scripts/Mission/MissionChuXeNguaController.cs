using System.Collections;
using DialogueEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MissionChuXeNguaController : MonoBehaviour, IConversationOverrideProvider
{
    [Header("References")]
    [SerializeField] private ConversationStarter conversationStarter;
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private VideoPlayer outroVideoPlayer;

    [Header("Conversations")]
    [SerializeField] private NPCConversation enoughMoneyConversation;
    [SerializeField] private NPCConversation reminderConversation;

    [Header("Money Requirement")]
    [SerializeField] private int requiredMoney = 50;

    [Header("Outro")]
    [SerializeField] private VideoClip outroClip;
    [SerializeField] private GameObject[] hideWhenOutroStarts;
    [SerializeField] private bool hideAllCanvasesOnOutro = true;
    [SerializeField] private string nextSceneName;

    private bool outroStarted;
    private Coroutine queuedOutroRoutine;

    private void Awake()
    {
        if (conversationStarter == null)
            conversationStarter = GetComponent<ConversationStarter>();

        if (outroVideoPlayer == null)
            outroVideoPlayer = GetComponentInChildren<VideoPlayer>(true);

        if (enoughMoneyConversation == null && conversationStarter != null)
            enoughMoneyConversation = conversationStarter.DefaultConversation;

        if (outroVideoPlayer != null)
        {
            outroVideoPlayer.playOnAwake = false;
            outroVideoPlayer.isLooping = false;
            outroVideoPlayer.loopPointReached += HandleOutroEnded;
        }
    }

    private void OnDisable()
    {
        if (queuedOutroRoutine != null)
        {
            StopCoroutine(queuedOutroRoutine);
            queuedOutroRoutine = null;
        }
    }

    private void OnDestroy()
    {
        if (outroVideoPlayer != null)
            outroVideoPlayer.loopPointReached -= HandleOutroEnded;
    }

    public bool TryGetConversationOverride(NPCConversation defaultConversation, out NPCConversation conversation)
    {
        conversation = defaultConversation;

        if (outroStarted)
            return false;

        if (HasEnoughMoney())
        {
            conversation = enoughMoneyConversation != null ? enoughMoneyConversation : defaultConversation;
            return conversation != null;
        }

        conversation = reminderConversation != null ? reminderConversation : defaultConversation;
        return conversation != null;
    }

    private bool HasEnoughMoney()
    {
        return moneyManager != null && moneyManager.currentMoney >= requiredMoney;
    }

    // Gắn method này vào EVENT của speech node cuối:
    // "Vậy hãy để đồ đã lên xe và cùng ta lên kinh thành nhé"
    public void MarkOutroReadyAfterFinalLine()
    {
        if (!HasEnoughMoney() || outroStarted)
            return;

        if (queuedOutroRoutine != null)
            StopCoroutine(queuedOutroRoutine);

        queuedOutroRoutine = StartCoroutine(WaitForConversationToCloseThenPlayOutro());
    }

    private IEnumerator WaitForConversationToCloseThenPlayOutro()
    {
        // Chờ frame hiện tại xử lý xong event
        yield return null;

        // Đợi tới khi hội thoại đóng hẳn (sau khi bấm End hoặc auto-end)
        while (ConversationManager.Instance != null && ConversationManager.Instance.IsConversationActive)
            yield return null;

        PlayOutro();
        queuedOutroRoutine = null;
    }

    public void PlayOutro()
    {
        if (outroStarted)
            return;

        if (outroVideoPlayer == null)
        {
            Debug.LogWarning("MissionChuXeNguaController: Outro VideoPlayer is not assigned.");
            return;
        }

        if (outroClip == null)
        {
            Debug.LogWarning("MissionChuXeNguaController: Outro clip is not assigned.");
            return;
        }

        outroStarted = true;

        if (hideWhenOutroStarts != null)
        {
            for (int i = 0; i < hideWhenOutroStarts.Length; i++)
            {
                if (hideWhenOutroStarts[i] != null)
                    hideWhenOutroStarts[i].SetActive(false);
            }
        }

        if (hideAllCanvasesOnOutro)
        {
            Canvas[] allCanvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
            for (int i = 0; i < allCanvases.Length; i++)
            {
                if (allCanvases[i] != null)
                    allCanvases[i].enabled = false;
            }
        }

        if (!outroVideoPlayer.gameObject.activeSelf)
            outroVideoPlayer.gameObject.SetActive(true);

        outroVideoPlayer.clip = outroClip;
        outroVideoPlayer.time = 0;
        outroVideoPlayer.Play();
    }

    private void HandleOutroEnded(VideoPlayer player)
    {
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
    }
}
