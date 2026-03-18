using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance; // Dùng Singleton để các script khác (như DropZone) dễ dàng gọi đến

    [Header("UI References")]
    public GameObject dragDropCanvas; // Kéo cái Canvas hoặc Panel chứa game kéo thả vào đây

    [Header("Player References")]
    public MonoBehaviour playerMovementScript; // Kéo script di chuyển của nhân vật vào đây để tạm tắt lúc chơi game

    private void Awake()
    {
        // Khởi tạo Singleton
        if (Instance == null) Instance = this;
    }

    // Hàm này gọi khi Bấm vào Option thoại của Bác Trưởng Làng
    public void OpenMiniGame()
    {
        dragDropCanvas.SetActive(true); // Hiện mini-game

        // Hiện và mở khóa con trỏ chuột để kéo đồ
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Tạm dừng di chuyển nhân vật (nếu có kéo script vào)
        if (playerMovementScript != null) playerMovementScript.enabled = false;
    }

    // Hàm này được DropZone gọi khi xếp đủ 5 quả
    public void CloseMiniGame()
    {
        dragDropCanvas.SetActive(false); // Ẩn mini-game

        // Ẩn và khóa chuột lại giữa màn hình để tiếp tục chơi 3D
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Bật lại di chuyển nhân vật
        if (playerMovementScript != null) playerMovementScript.enabled = true;
    }
}