// ScreenFader.cs — shared by anything that needs a full-screen fade
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class ScreenFader
{
    public static CanvasGroup CreateOverlay(float startAlpha)
    {
        GameObject fadeObject = new GameObject("ScreenFadeOverlay");
        Canvas canvas = fadeObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000;

        CanvasGroup canvasGroup = fadeObject.AddComponent<CanvasGroup>();
        Image image = fadeObject.AddComponent<Image>();
        image.color = Color.black;
        image.raycastTarget = false;

        RectTransform rt = image.rectTransform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        canvasGroup.alpha = startAlpha;
        return canvasGroup;
    }

    public static IEnumerator Fade(CanvasGroup group, float from, float to, float duration)
{
    if (group == null) yield break;
    group.alpha = from;

    const float maxStep = 0.05f; // ~20fps worth — caps any single-frame spike

    float elapsed = 0f;
    while (elapsed < duration)
    {
        if (group == null) yield break;
        elapsed += Mathf.Min(Time.unscaledDeltaTime, maxStep);
        group.alpha = Mathf.Lerp(from, to, elapsed / duration);
        yield return null;
    }
    if (group != null) group.alpha = to;
}
}