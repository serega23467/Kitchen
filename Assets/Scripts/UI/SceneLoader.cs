using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    RectTransform rotatedThing;
    [SerializeField]
    TMP_Text progressText;
    public static bool PlayHideOnAwake = false;

    static SceneLoader instance;
    public static SceneLoader Instance { get => instance; }
    AsyncOperation loadingSceneOperation;
    CanvasGroup canvasGroup;

    Tween showAnim;
    Tween hideAnim;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this);
    }
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        showAnim = canvasGroup.DOFade(1, 1f).SetAutoKill(false);
        hideAnim = canvasGroup.DOFade(0, 1f).SetAutoKill(false).OnComplete(()=>gameObject.SetActive(false));

        if (PlayHideOnAwake)
        {
            Hide();
            PlayHideOnAwake = false;
        }
        else
        {
            canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }

    }
    void Update()
    {
        rotatedThing.Rotate(0, 0, -1 * Time.deltaTime * 500);
        if (loadingSceneOperation != null)
        {
            progressText.text = "Загрузка " + Mathf.RoundToInt(loadingSceneOperation.progress * 100f) + " %";
        }
    }
    void Hide()
    {
       hideAnim.Restart();
    }
    public static void SwitchScene(string sceneName)
    {
        if (instance == null) return;

        instance.gameObject.SetActive(true);
        instance.showAnim.onComplete = () => { instance.loadingSceneOperation.allowSceneActivation = true; };
        instance.showAnim.Restart();

        instance.loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);
        instance.loadingSceneOperation.allowSceneActivation = false;

    }
}
