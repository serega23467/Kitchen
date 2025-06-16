using System.Data;
using TMPro;
using UnityEngine;

public class PanelRecipe : MonoBehaviour
{
    [SerializeField]
    TMP_Text recipeText;

    RectTransform panelRect;
    Vector3 size = Vector3.zero;
    private void Awake()
    {       
        panelRect = GetComponent<RectTransform>();
        size = panelRect.localScale;
    }
    private void Start()
    {
        recipeText.text = BellFinish.Level.RecipeText;
        recipeText.fontSize = UIElements.GetFontSize(recipeText.text.Length);
        Hide();
    }
    public void Show()
    {
        panelRect.localScale = size;
    }
    public void Hide()
    {
        panelRect.localScale = Vector3.zero;
    }
}
