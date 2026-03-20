using UnityEngine;
using GinjaGaming.FinalCharacterController; 

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance;
    public GameObject dragDropCanvas;
    public GameObject playerObject;

    private Inventory inventoryScript; 
    private void Awake() => Instance = this;

    private void Start()
    {
        inventoryScript = FindAnyObjectByType<Inventory>();
    }

    public void OpenMiniGame()
    {
        dragDropCanvas.SetActive(true);

      
        if (inventoryScript != null)
        {
            inventoryScript.enabled = false;
        }

        PlayerController controller = playerObject.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.SetCameraControlEnabled(false);
            controller.enabled = false;
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseMiniGame()
    {
        dragDropCanvas.SetActive(false);

        if (inventoryScript != null)
        {
            inventoryScript.enabled = true;
        }

        PlayerController controller = playerObject.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.enabled = true;
            controller.SetCameraControlEnabled(true);
        }
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}