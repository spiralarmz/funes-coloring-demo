using UnityEngine;

namespace Funes.Painting
{
    /// <summary>
    /// Brush probe: paints on CONTACT — wherever the brush tip touches a surface,
    /// at any angle — with no trigger. Grab the brush and paint; that's it. Pair with
    /// a <see cref="PaintTool"/> in Brush mode and a SHORT maxDistance so it only marks
    /// where the bristles actually touch. The precision counterpart to the ranged
    /// spray: it reaches the recesses and edges the spray cone can't (two-stage reveal).
    /// </summary>
    public class ContactPaintProbe : PaintProbe
    {
        [Tooltip("Brush tip; paints along its +Z (forward). Defaults to this transform.")]
        [SerializeField] Transform tip;

        public override bool TryGetAimRay(out Ray ray)
        {
            Transform origin = tip != null ? tip : transform;
            ray = new Ray(origin.position, origin.forward);
            return true;
        }

        public override bool TryGetPaintRay(out Ray ray, out float activation)
        {
            // Always "on": contact gating is handled by the PaintTool's short
            // maxDistance — the forward ray only finds a surface when the tip is
            // right against it. No trigger, no loaded-brush mode.
            Transform origin = tip != null ? tip : transform;
            ray = new Ray(origin.position, origin.forward);
            activation = 1f;
            return true;
        }
    }
}
