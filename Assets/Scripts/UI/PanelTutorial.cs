using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelTutorial : MonoBehaviour
{
    [SerializeField]
    Image slideImage;
    [SerializeField]
    TMP_Text currentText;
    [SerializeField]
    TMP_Text countText;

    Sprite[] sprites;
    string[] texts;
    int currentIndex = 0;
    void Awake()
    {
        sprites = Resources.LoadAll<Sprite>("Tutorials/Sprites")?.OrderBy(s=>int.Parse(s.name)).ToArray();
        texts = Resources.LoadAll<TextAsset>("Tutorials/Texts")?.OrderBy(t => int.Parse(t.name)).Select(t=>t.text).ToArray();

        if (sprites.Length > 0)
        {
            slideImage.sprite = sprites[currentIndex];
            SetText(texts[currentIndex]);
        }
    }
    private void Start()
    {
        Hide();
    }
    public void NextImage()
    {
        currentIndex++;
        if (currentIndex >= sprites.Length)
        {
            currentIndex = 0;
        }

        slideImage.sprite = sprites[currentIndex];
        SetText(texts[currentIndex]);
    }
    public void BackImage()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = sprites.Length - 1;
        }
        slideImage.sprite = sprites[currentIndex];
        SetText(texts[currentIndex]);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    void SetText(string text)
    {
        currentText.text = Translator.ReplaceActionToKey(text);
        countText.text = $"{currentIndex + 1}/{sprites.Length}";
    }
}
