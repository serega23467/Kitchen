using Assets.Scripts.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

public class SliderMenu : MonoBehaviour, IHideble
{
    [SerializeField]
    Slider sliderCount;
    [SerializeField]
    TMP_Text countValue;
    [SerializeField]
    Button button;
    public bool IsActive => gameObject.activeSelf;
    void Start()
    {
        sliderCount.minValue = 0;
        sliderCount.maxValue = 1;
        sliderCount.onValueChanged.AddListener(UpdateSliderValue);
        UpdateSliderValue(sliderCount.value);

        Hide();
    }
    public void OpenSliderMenu(Action<List<FoodComponent>, int> onSelect, List<FoodComponent> list)
    {
        sliderCount.minValue = 0;
        sliderCount.maxValue = list.Count();
        sliderCount.value = sliderCount.maxValue;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate() { onSelect.Invoke(list, Convert.ToInt32(sliderCount.value));});
        button.onClick.AddListener(Hide);
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        sliderCount.value = 0;
        gameObject.SetActive(false);
    }
    void UpdateSliderValue(float value)
    {
        countValue.text = Convert.ToInt32(value).ToString();
    }
}
