using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSettingClick : MonoBehaviour
{
    public static ButtonSettingClick instance { get; private set; }

    Button _setting;

    public delegate void ClickButtonSetting();
    public ClickButtonSetting callbackClickBtSetting { get; private set; }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void Init(ClickButtonSetting callbackClickBtSetting)
    {
        this.callbackClickBtSetting = null;
        this.callbackClickBtSetting = callbackClickBtSetting;
        _setting = GetComponent<Button>();
        _setting.onClick.RemoveAllListeners();
        _setting.onClick.AddListener(Onclick);
    }

    public void Onclick() => callbackClickBtSetting?.Invoke();
}
