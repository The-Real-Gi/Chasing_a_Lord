using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIRaycastDebugger : MonoBehaviour
{
    [SerializeField] private bool logEveryMouseClick = true;

    private void OnEnable()
    {
        Debug.Log("UI raycast debugger is active.", this);
    }

    private void OnGUI()
    {
        if (!logEveryMouseClick || Event.current == null)
        {
            return;
        }

        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Vector2 screenPosition = new Vector2(
                Event.current.mousePosition.x,
                Screen.height - Event.current.mousePosition.y);

            Debug.Log("UI raycast debugger detected a pointer click.", this);
            LogUIObjectsUnderPointer(screenPosition);
        }
    }

    public void LogCurrentPointer()
    {
        if (Pointer.current != null)
        {
            LogUIObjectsUnderPointer(Pointer.current.position.ReadValue());
        }
    }

    private void LogUIObjectsUnderPointer(Vector2 screenPosition)
    {
        if (EventSystem.current == null)
        {
            Debug.LogWarning("UI raycast debug: no active EventSystem exists in this scene.");
            return;
        }

        var pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        var hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, hits);

        if (hits.Count == 0)
        {
            Debug.Log("UI raycast debug: nothing received the pointer at " + screenPosition + ".");
            return;
        }

        Debug.Log("UI raycast debug: objects under pointer, front to back:");

        for (int i = 0; i < hits.Count; i++)
        {
            var hit = hits[i];
            var graphic = hit.gameObject.GetComponent<UnityEngine.UI.Graphic>();
            var canvasGroup = hit.gameObject.GetComponent<CanvasGroup>();
            var button = hit.gameObject.GetComponentInParent<Button>();

            string details = "  [" + i + "] " + hit.gameObject.name;

            if (graphic != null)
            {
                details += " | " + graphic.GetType().Name
                    + " | Raycast Target=" + graphic.raycastTarget;
            }

            if (canvasGroup != null)
            {
                details += " | CanvasGroup Blocks Raycasts=" + canvasGroup.blocksRaycasts
                    + " | Interactable=" + canvasGroup.interactable
                    + " | Alpha=" + canvasGroup.alpha;
            }

            if (button != null)
            {
                details += " | Button=" + button.name
                    + " | Button Interactable=" + button.IsInteractable()
                    + " | OnClick Listeners=" + button.onClick.GetPersistentEventCount();
            }

            Debug.Log(details, hit.gameObject);
        }
    }
}
