using Assets.Scripts.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

public class SliderMenu : MonoBehaviour
{
    public bool IsOpen { get; private set; }
    [SerializeField]
    Slider sliderCount;
    [SerializeField]
    TMP_Text countValue;
    [SerializeField]
    Button button;

    RectTransform rectTransform;
    Vector3 menuSize;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        menuSize = rectTransform.localScale;
    }
    void Start()
    {
        sliderCount.minValue = 0;
        sliderCount.maxValue = 1;
        sliderCount.onValueChanged.AddListener(UpdateSliderValue);
        UpdateSliderValue(sliderCount.value);
    }
    public void OpenSliderMenu(Action<List<FoodComponent>, int> onSelect, List<FoodComponent> list)
    {
        sliderCount.minValue = 0;
        sliderCount.maxValue = list.Count();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate() { onSelect.Invoke(list, Convert.ToInt32(sliderCount.value));});
        button.onClick.AddListener(CloseSliderMenu);
        rectTransform.localScale = menuSize;
        IsOpen = true;
    }
    public void CloseSliderMenu()
    {
        rectTransform.localScale = Vector3.zero;
        sliderCount.value = 0;
        IsOpen = false;
    }
    void UpdateSliderValue(float value)
    {
        countValue.text = Convert.ToInt32(value).ToString();
    }
}
