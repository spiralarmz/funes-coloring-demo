using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

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

        [Header("Haptics")]
        [Tooltip("Peak rumble amplitude at full trigger pressure (0 disables haptics).")]
        [SerializeField, Range(0f, 1f)] float hapticAmplitude = 0.25f;

        const float HapticPulseDuration = 0.05f;
        const float HapticResendInterval = 0.04f; // overlapping pulses read as one soft continuous buzz
        float _nextHapticTime;

        void OnEnable()  { activateAction.action?.Enable(); }
        void OnDisable() { activateAction.action?.Disable(); }

        public override void NotifyPaintApplied(float activation)
        {
            if (hapticAmplitude <= 0f || Time.unscaledTime < _nextHapticTime) return;

            // The control that produced the trigger value belongs to the controller
            // we should rumble — works for either hand without extra configuration.
            var device = activateAction.action?.activeControl?.device;
            if (device is XRControllerWithRumble rumble)
            {
                rumble.SendImpulse(hapticAmplitude * Mathf.Clamp01(activation), HapticPulseDuration);
                _nextHapticTime = Time.unscaledTime + HapticResendInterval;
            }
        }

        public override bool TryGetAimRay(out Ray ray)
        {
            // A held tool always has an aim pose, trigger or not — lets the aim
            // line show while lining up a spray.
            Transform origin = tip != null ? tip : transform;
            ray = new Ray(origin.position, origin.forward);
            return true;
        }

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
