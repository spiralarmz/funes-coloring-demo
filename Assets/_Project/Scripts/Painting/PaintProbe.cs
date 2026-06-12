using UnityEngine;

namespace Funes.Painting
{
    /// <summary>
    /// Input-agnostic source of "where and how hard to paint". Concrete probes
    /// (VR tool, mouse, future touch) feed <see cref="PaintTool"/> the same data,
    /// keeping the paint/reveal logic decoupled from any specific input device.
    /// This is the seam that keeps a non-VR web build (and other XR runtimes) viable.
    /// </summary>
    public abstract class PaintProbe : MonoBehaviour
    {
        /// <summary>
        /// Returns a world-space ray to paint along plus an activation (0..1,
        /// e.g. trigger pressure). Return false when not painting this frame.
        /// </summary>
        public abstract bool TryGetPaintRay(out Ray ray, out float activation);

        /// <summary>
        /// Returns the tool's current aim ray even when not painting (e.g. for an
        /// aim line while the trigger is released). Default: only available while
        /// painting. Probes with a persistent pose (a held VR tool) should override.
        /// </summary>
        public virtual bool TryGetAimRay(out Ray ray) => TryGetPaintRay(out ray, out _);

        /// <summary>
        /// Called by PaintTool only when paint actually landed on a surface this
        /// frame. Probes translate this into device-appropriate feedback (e.g.
        /// controller haptics); the default is no feedback.
        /// </summary>
        public virtual void NotifyPaintApplied(float activation) {}
    }
}
