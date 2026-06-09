using UnityEngine;
using UnityEngine.InputSystem;

namespace Funes.Painting
{
    /// <summary>
    /// Desktop/editor probe: paints from the camera through the mouse cursor
    /// while the left mouse button is held. Lets us validate the reveal loop
    /// without a headset — and proves the <see cref="PaintProbe"/> abstraction
    /// (same PaintTool, different input driver).
    /// </summary>
    public class MousePaintProbe : PaintProbe
    {
        [SerializeField] Camera sourceCamera;

        void Awake()
        {
            if (sourceCamera == null) sourceCamera = Camera.main;
        }

        public override bool TryGetPaintRay(out Ray ray, out float activation)
        {
            ray = default;
            activation = 0f;

            var mouse = Mouse.current;
            if (mouse == null || sourceCamera == null) return false;
            if (!mouse.leftButton.isPressed) return false;

            ray = sourceCamera.ScreenPointToRay(mouse.position.ReadValue());
            activation = 1f;
            return true;
        }
    }
}
