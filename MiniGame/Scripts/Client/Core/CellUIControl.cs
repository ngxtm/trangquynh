using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CellUIControl : MonoBehaviour
{
    [SerializeField] SerializableIntListImageDictionary _dicStones = new SerializableIntListImageDictionary();
    [SerializeField] List<Text> _cellTexts = new List<Text>();
    [SerializeField] List<Button> _cellButtons = new List<Button>();
    [SerializeField] GameObject _Quan1Image;
    [SerializeField] GameObject _Quan2Image;

    public delegate void CallbackOnClickDan(int idx);
    public CallbackOnClickDan _onClickDan;
    public void Initial(CallbackOnClickDan onClickDan)
    {
        _dicStones.Clear();
        _cellTexts.Clear();
        _cellButtons.Clear();
        _onClickDan = null;
        _onClickDan = onClickDan;

        Transform board = transform.Find("board");
        Transform parentDan1 = board.Find("Dan1");
        Transform parentDan2 = board.Find("Dan2");
        Transform parentQuan = board.Find("Quan");


        // Add itemDan1
        for (int i = 0; i < 5; i++)
        {
            int idx = i;
            Transform danCell = parentDan1.GetChild(i);
            Button danBT = danCell.GetComponent<Button>();
            danBT.onClick.AddListener(() => OnClickDan(idx));
            _cellButtons.Add(danBT);
            _cellTexts.Add(danCell.GetComponentInChildren<Text>());

            Transform itemDanParent = danCell.Find("item_cell_1");
            List<Image> itemStonesDan1 = new List<Image>();
            foreach (Transform child in itemDanParent)
                itemStonesDan1.Add(child.GetComponent<Image>());
            _dicStones.Add(idx, itemStonesDan1);
        }

        // Add Quan1
        _cellTexts.Add(parentQuan.GetChild(0).GetComponentInChildren<Text>());
        Transform itemQuan1Parent = parentQuan.GetChild(0).Find("item/da");
        _Quan1Image = parentQuan.GetChild(0).Find("item/quan/itemquan").gameObject;
        List<Image> itemStonesQuan1 = new List<Image>();
        foreach (Transform child in itemQuan1Parent)
            itemStonesQuan1.Add(child.GetComponent<Image>());
        _dicStones.Add(5, itemStonesQuan1);

        // Add itenDan2 (6-10)
        for (int i = 0; i < 5; i++)
        {
            int idx = i + 6; // index Dan 2 start = 6
            Transform danCell = parentDan2.GetChild(i);
            Button danBT = danCell.GetComponent<Button>();
            danBT.onClick.AddListener(() => OnClickDan(idx));
            _cellButtons.Add(danBT);
            _cellTexts.Add(danCell.GetComponentInChildren<Text>());

            Transform itemDanParent = danCell.Find("item_cell_2");
            List<Image> itemStonesDan2 = new List<Image>();
            foreach (Transform child in itemDanParent)
                itemStonesDan2.Add(child.GetComponent<Image>());
            _dicStones.Add(idx, itemStonesDan2);
        }

        // Add Quan 2
        _cellTexts.Add(parentQuan.GetChild(1).GetComponentInChildren<Text>());
        Transform itemQuan2Parent = parentQuan.GetChild(1).Find("item/da");
        _Quan2Image = parentQuan.GetChild(1).Find("item/quan/itemquan").gameObject;
        print(itemQuan2Parent.name);
        List<Image> itemStonesQuan2 = new List<Image>();
        foreach (Transform child in itemQuan2Parent)
            itemStonesQuan2.Add(child.GetComponent<Image>());
        _dicStones.Add(11, itemStonesQuan2);
    }

    public void UpdateBoard(int[] stones)
    {   
        for (int i = 0; i < 12; i++)
        {
            var sb = StringBuilderCache.Acquire(5);
            sb.Append(stones[i]);
            _cellTexts[i].text = StringBuilderCache.GetStringAndRelease(sb);
            
            if (_dicStones.TryGetValue(i, out List<Image> images))
            {
                int count = stones[i];
                // Nếu là ô Quan (5 hoặc 11), trừ đi 1 viên đá Quan 
                if (i == 5 && GameManager.instance.BoardManager.Quan1Available || i == 11 && GameManager.instance.BoardManager.Quan2Available)
                    count = Mathf.Max(0, count - 1);

                for (int j = 0; j < images.Count; j++)
                {
                    images[j].gameObject.SetActive(j < count);
                }

                _Quan1Image.SetActive(GameManager.instance.BoardManager.Quan1Available);
                _Quan2Image.SetActive(GameManager.instance.BoardManager.Quan2Available);
            }
        }
    }

    public void OnClickDan(int idx) => _onClickDan?.Invoke(idx);

    public void PulseCell(int cellIndex)
    {
        if (cellIndex < 0 || cellIndex >= _cellTexts.Count) return;
        
        Text cellText = _cellTexts[cellIndex];
        if (cellText != null)
        {
            VFXControl._instance?.ScaleImage(cellText, 1.3f, GameConstants.PULSE_DURATION, GameConstants.COLOR_HIGHLIGHT);
        }
    }

}
