using UnityEngine;

/// <summary>
/// Gắn script này vào NPC trong game lớn.
/// NPC cần có Collider (isTrigger = true).
/// Khi người chơi đến gần và bấm E → mở mini-game Ô Ăn Quan.
/// </summary>
public class OQuanLauncher : MonoBehaviour
{
    [Header("Kéo thả OQuanRoot vào đây")]
    public GameObject oquanRoot;

    [Header("Tắt game lớn khi chơi mini-game (tuỳ chọn)")]
    public GameObject mainGameUI;           // Canvas UI game lớn
    public Camera mainCamera;               // Camera chính
    public MonoBehaviour playerController;  // Script điều khiển nhân vật

    [Header("Cài đặt Trigger")]
    public KeyCode activateKey = KeyCode.E; // Phím bấm để chơi
    public GameObject promptUI;             // UI hiện chữ "Bấm E để chơi" (tuỳ chọn)

    private bool _playerInRange = false;
    private bool _isPlaying = false;

    void Update()
    {
        // Khi người chơi đang ở gần NPC và bấm phím E
        if (_playerInRange && !_isPlaying && Input.GetKeyDown(activateKey))
        {
            LaunchMiniGame();
        }
    }

    /// <summary>
    /// Người chơi đi vào vùng trigger của NPC
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = true;
            if (promptUI != null) promptUI.SetActive(true);
            Debug.Log("📍 Đến gần NPC — Bấm E để chơi Ô Ăn Quan!");
        }
    }

    /// <summary>
    /// Người chơi rời khỏi vùng trigger
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = false;
            if (promptUI != null) promptUI.SetActive(false);
        }
    }

    /// <summary>
    /// Bật mini-game
    /// </summary>
    public void LaunchMiniGame()
    {
        if (oquanRoot == null)
        {
            Debug.LogError("❌ OQuanLauncher: oquanRoot chưa được gán!");
            return;
        }

        _isPlaying = true;

        // Tạm ẩn game lớn
        if (mainGameUI != null) mainGameUI.SetActive(false);
        if (playerController != null) playerController.enabled = false;
        if (promptUI != null) promptUI.SetActive(false);

        // Khoá con trỏ chuột (nếu game FPS dùng Cursor.lockState)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Đăng ký callback khi mini-game kết thúc
        OQuanBridge.OnMiniGameEnd = OnMiniGameFinished;

        // Bật mini-game
        oquanRoot.SetActive(true);

        Debug.Log("🎮 Mini-game Ô Ăn Quan đã khởi động!");
    }

    /// <summary>
    /// Được gọi tự động khi mini-game kết thúc (qua OQuanBridge)
    /// </summary>
    private void OnMiniGameFinished(bool playerWon, int playerScore, int aiScore)
    {
        _isPlaying = false;

        // Tắt mini-game
        oquanRoot.SetActive(false);

        // Bật lại game lớn
        if (mainGameUI != null) mainGameUI.SetActive(true);
        if (playerController != null) playerController.enabled = true;

        // Khoá lại con trỏ (nếu game FPS)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log($"🏆 Mini-game kết thúc! Thắng: {playerWon}, Điểm: {playerScore} vs {aiScore}");

        // TODO: Thêm logic phần thưởng tuỳ game của bạn
        // if (playerWon) { InventoryManager.Instance.AddGold(50); }
    }
}
