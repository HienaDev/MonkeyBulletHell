using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [SerializeField] private RectTransform uiElement; // Reference to the UI element


    private void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        if (uiElement != null)
        {
            // Convert the mouse position to a position in the canvas
            Vector2 mousePosition = Input.mousePosition;

            // Update the position of the UI element
            uiElement.position = mousePosition;
        }
    }
}
