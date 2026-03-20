using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SlashScreenControl : MonoBehaviour
{
    public static SlashScreenControl instance;
    [SerializeField] Sprite[] sprites;
    public Sprite[] Sprites => sprites;
    public Image splash;
    public GameObject slash_screen_UI;
    public GameObject fill_Bar;
    public Image fill;
    float max_frame;
    float max_volume;
    public Text log_game;

    private void Awake()
    {
        instance = this;
        Show(true, RandomImage(), 1);
    }
    
    public void Show(bool fill_bar, int image, float opacity)
    {
        slash_screen_UI.SetActive(true);
        if (image != -1)
        {
            splash.color = Color.white;
            splash.sprite = sprites[image];
        }
        else
        {
            splash.color = Color.black;
            splash.sprite = null;
        }
        fill_Bar.SetActive(fill_bar);
        if (fill_bar) UpdateFillBar(0);
        SetOpacity(opacity);
    }

    public void UpdateFillBar(float amount)
        => fill.fillAmount = amount;

    public int RandomImage()
        => Random.Range(0, sprites.Length);

    public void Hide()
    {
        slash_screen_UI.SetActive(false);
        Debug.Log("🔴 SlashScreen hidden - slash_screen_UI.SetActive(false)");
    }

    public void SetOpacity(float amount)
        => splash.color
        = fill_Bar.GetComponent<Image>().color
        = fill.color
        = new Color(splash.color.r, splash.color.g, splash.color.b, amount);

    public IEnumerator ShowAnimated(bool fill_bar, int image, int frame, int callback_value, System.Action<int> CallBack)
    {
        if (!slash_screen_UI.activeSelf)
        {
            Show(fill_bar, image, 0);
            max_frame = frame;
            while (frame > 0)
            {
                frame--;
                float f = 1 - (float)frame / max_frame;
                SetOpacity(Mathf.Clamp01(f));
                yield return new WaitForEndOfFrame();
            }
        }
        CallBack?.Invoke(callback_value);
    }

    public IEnumerator HideAnimated(int frame, int callback_value, System.Action<int> CallBack)
    {
        max_frame = frame;
        while (frame > 0)
        {
            frame--;
            float f = (float)frame / max_frame;
            SetOpacity(Mathf.Clamp01(f));
            yield return new WaitForEndOfFrame();
        }
        Hide();
        CallBack?.Invoke(callback_value);
    }
}
