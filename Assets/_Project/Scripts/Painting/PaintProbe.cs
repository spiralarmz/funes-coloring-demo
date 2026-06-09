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
    }
}
