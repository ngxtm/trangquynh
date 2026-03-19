using UnityEngine;
using GinjaGaming.FinalCharacterController; // Quan trọng: Phải có dòng này để gọi được PlayerController

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance;
    public GameObject dragDropCanvas;
    public GameObject playerObject; // Kéo object "Player" vào đây

    private void Awake() => Instance = this;

    public void OpenMiniGame()
    {
        dragDropCanvas.SetActive(true);

        // Lấy script Controller từ nhân vật
        PlayerController controller = playerObject.GetComponent<PlayerController>();
        if (controller != null)
        {
            // Tắt quyền điều khiển Camera (hàm này có sẵn trong code của bạn)
            controller.SetCameraControlEnabled(false);
            // Tắt luôn script để chắc chắn không nhận click chuột
            controller.enabled = false;
        }

        // Hiện chuột và mở khóa
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseMiniGame()
    {
        dragDropCanvas.SetActive(false);

        PlayerController controller = playerObject.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.enabled = true;
            controller.SetCameraControlEnabled(true);
        }
        
        // Khóa lại chuột khi quay về game 3D
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}