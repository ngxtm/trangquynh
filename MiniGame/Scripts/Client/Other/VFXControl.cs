using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class VFXControl : MonoBehaviour
{
    public static VFXControl _instance;
    [Header("Prefab chứa Image và Text")]
    public GameObject _vfxPrefab; // Prefab phải có Image và Text

    [Header("Vị trí spawn")]
    public Transform _spawnParent;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    /// <summary>
    /// Hiển thị VFX bay lên với text và sprite truyền vào
    /// </summary>
    /// <param name="text">Nội dung hiển thị</param>
    /// <param name="sprite">Sprite hiển thị</param>
    /// <param name="distance">Khoảng cách bay lên (pixel)</param>
    /// <param name="duration">Thời gian bay (giây)</param>
    public void ShowVFX(string text, Sprite sprite, Quaternion rotationVFX, float distance = 150f, float duration = 1f, Transform spawnParent = null)
    {
        GameObject vfx = Instantiate(_vfxPrefab, spawnParent ?? _spawnParent);
        vfx.SetActive(true);

        // Gán text và sprite
        var img = vfx.GetComponentInChildren<Image>();
        var txt = vfx.GetComponentInChildren<Text>();
        if (img != null)
        {
            img.sprite = sprite;
            RectTransform rect = img.GetComponent<RectTransform>();
            rect.localRotation = rotationVFX;
        }
        if (txt != null)
            txt.text = text;

        // Bắt đầu hiệu ứng bay lên
        StartCoroutine(FlyUpAndHide(vfx, distance, duration));
    }

    #region Fly Up Effect
    /// <summary>
    /// Hiệu ứng bay lên và ẩn VFX
    IEnumerator FlyUpAndHide(GameObject vfx, float distance, float duration)
    {
        RectTransform rect = vfx.GetComponent<RectTransform>();
        Vector3 startPos = rect.anchoredPosition;
        Vector3 endPos = startPos + Vector3.up * distance;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            // Tính tỉ lệ tiến trình
            float t = elapsed / duration;
            // Tăng tốc khi gần đến (ease-in)
            float speed = Mathf.Lerp(1.5f, 3.5f, t); // nhỏ thì nhanh, gần đến thì càng nhanh
            rect.anchoredPosition = Vector3.Lerp(startPos, endPos, Mathf.Pow(t, speed));
            elapsed += Time.deltaTime;
            yield return null;
        }
        rect.anchoredPosition = endPos;
        vfx.SetActive(false);
        Destroy(vfx, 0.2f); // Xoá sau khi ẩn
    }
    #endregion

    #region Scale Effect
    Coroutine _coroutineScale;
    Dictionary<Text, Vector3> originalScales = new(); // Lưu original scale của từng Text


    /// <summary>
    /// Scale Image với hiệu ứng phóng to/thu nhỏ + đổi màu tạm thời (nếu có truyền)
    /// </summary>
    /// <param name="text">Text cần hiệu ứng</param>
    /// <param name="scaleFactor">Tỷ lệ phóng to</param>
    /// <param name="duration">Thời gian toàn bộ hiệu ứng</param>
    /// <param name="colorHex">Màu dạng "#00FF00" (green) hoặc null nếu không đổi màu</param>
    public void ScaleImage(Text text, float scaleFactor, float duration, string colorHex = null)
    {
        /*  if (_coroutineScale != null)
             StopCoroutine(_coroutineScale);
         _coroutineScale = StartCoroutine(AnimateText(text, duration, scaleFactor, colorHex)); */
        if (!originalScales.ContainsKey(text))
            originalScales[text] = text.rectTransform.localScale;

        if (_coroutineScale != null)
            StopCoroutine(_coroutineScale);
        _coroutineScale = StartCoroutine(AnimateText(text, duration, scaleFactor, colorHex));

    }

    IEnumerator AnimateText(Text text, float duration, float scaleTarget, string colorHex = null)
    {
        RectTransform tf = text.rectTransform;
        Vector3 originalScale = originalScales[text]; // dùng scale gốc thực sự
        Vector3 maxScale = originalScale * scaleTarget;

        Color originalColor = text.color;
        Color targetColor = originalColor;

        // Nếu có truyền mã màu hợp lệ → đổi sang màu đó
        if (!string.IsNullOrEmpty(colorHex) && ColorUtility.TryParseHtmlString(colorHex, out Color parsedColor))
        {
            targetColor = parsedColor;
        }

        float half = duration / 2f;
        float timer = 0;

        // Scale up + đổi màu
        while (timer < half)
        {
            timer += Time.deltaTime;
            float t = timer / half;
            tf.localScale = Vector3.Lerp(originalScale, maxScale, t);
            text.color = Color.Lerp(originalColor, targetColor, t);
            yield return null;
        }

        timer = 0;
        // Scale back + đổi màu về
        while (timer < half)
        {
            timer += Time.deltaTime;
            float t = timer / half;
            tf.localScale = Vector3.Lerp(maxScale, originalScale, t);
            text.color = Color.Lerp(targetColor, originalColor, t);
            yield return null;
        }

        tf.localScale = originalScale;
        text.color = originalColor;
    }
    #endregion


    void OnDisable()
    {
        if (_coroutineScale != null)
            StopCoroutine(_coroutineScale);
    }

    void OnDestroy()
    {
        if (_coroutineScale != null)
            StopCoroutine(_coroutineScale);
    }
}