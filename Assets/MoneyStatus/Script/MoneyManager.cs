using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    [Header("Money Stats")]
    public int currentMoney = 0;
    public int targetMoney = 50;  // Cố định mục tiêu là 50

    [Header("UI Text References")]
    public Text currentValueText;
    public Text targetValueText;  // Kéo cục 'Value' vào đây

    void Start()
    {
        // Hiển thị mục tiêu 50 lên màn hình ngay từ đầu
        if (targetValueText != null) targetValueText.text = targetMoney.ToString();
        UpdateMoneyUI();
    }

    // HÀM NÀY SẼ ĐƯỢC NPC GỌI KHI TRẢ NHIỆM VỤ
    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateMoneyUI();

        // Kiểm tra xem đã đủ 50 chưa để qua màn
        if (currentMoney >= targetMoney)
        {
            currentMoney = targetMoney; // Khóa lại ở mức 50 cho đẹp UI (tùy chọn)
            LevelComplete();
        }
    }

    private void UpdateMoneyUI()
    {
        if (currentValueText != null) currentValueText.text = currentMoney.ToString();
    }

    private void LevelComplete()
    {
        Debug.Log("🎉 ĐÃ ĐẠT 50 VÀNG! QUA MÀN THÀNH CÔNG!");
        // Ở đây sau này bạn có thể gọi hàm hiện UI Win Game hoặc Load Scene mới
        // VD: SceneManager.LoadScene("Level_2");
    }
}