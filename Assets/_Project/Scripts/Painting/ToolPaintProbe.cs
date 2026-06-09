using UnityEngine;
using UnityEngine.InputSystem;

namespace Funes.Painting
{
    /// <summary>
    /// VR probe: paints along a tool tip's forward axis (spray nozzle / brush
    /// tip), with activation read from an Input System action (controller
    /// trigger). The action is the abstraction seam — rebind it for hand
    /// tracking, other XR runtimes, or touch without touching paint logic.
    /// </summary>
    public class ToolPaintProbe : PaintProbe
    {
        [Tooltip("Tip of the tool; paints along its +Z (forward). Defaults to this transform.")]
        [SerializeField] Transform tip;

        [Tooltip("Float (0..1) activation action, e.g. <XRController>/trigger.")]
        [SerializeField] InputActionProperty activateAction;

        [Tooltip("Below this activation value, no paint is applied.")]
        [SerializeField, Range(0f, 1f)] float threshold = 0.1f;

        void OnEnable()  { activateAction.action?.Enable(); }
        void OnDisable() { activateAction.action?.Disable(); }

        public override bool TryGetPaintRay(out Ray ray, out float activation)
        {
            ray = default;
            activation = 0f;

            Transform origin = tip != null ? tip : transform;
            var action = activateAction.action;
            activation = action != null ? action.ReadValue<float>() : 0f;
            if (activation < threshold) return false;

            ray = new Ray(origin.position, origin.forward);
            return true;
        }
    }
}
