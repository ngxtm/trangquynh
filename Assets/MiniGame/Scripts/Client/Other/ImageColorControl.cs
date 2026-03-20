using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageColorControl : MonoBehaviour
{
    public static void ChangeImageColor(string hexColor, ref Image image)
    {
        // Chuyển đổi mã màu hexadecimal sang Color
        if (ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
        {   
            // Kiểm tra xem đối tượng Image có tồn tại không
            if (image != null)
            {   
                image.color = newColor;
            }
            else
            {
                Debug.LogError("Image reference is null!");
            }
        }
        else
        {
            Debug.LogError("Invalid hexadecimal color code!");
        }
    }
}
