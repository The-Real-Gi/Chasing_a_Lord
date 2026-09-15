using UnityEngine;
using UnityEngine.UI;

public class LevelWinCondition : MonoBehaviour
{
    public LayerMask player;
    [SerializeField] private float fadeDuration = 1f;

    private bool isTransitioning;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTransitioning && collision.CompareTag("Player"))
        {   
            isTransitioning = true;
            StartCoroutine(FadeAndLoadNextLevel());
        }
    }

    System.Collections.IEnumerator FadeAndLoadNextLevel()
{
    CanvasGroup fadeCanvasGroup = ScreenFader.CreateOverlay(0f);
    yield return ScreenFader.Fade(fadeCanvasGroup, 0f, 1f, fadeDuration);
    GameManager.Instance.LoadNextLevel();
}

    CanvasGroup CreateFadeOverlay()
    {
        GameObject fadeObject = new GameObject("LevelTransitionFade");
        Canvas canvas = fadeObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000;

        CanvasGroup canvasGroup = fadeObject.AddComponent<CanvasGroup>();
        Image image = fadeObject.AddComponent<Image>();
        image.color = Color.black;
        image.raycastTarget = false;

        RectTransform rectTransform = image.rectTransform;
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        canvasGroup.alpha = 0f;
        return canvasGroup;
    }
}
