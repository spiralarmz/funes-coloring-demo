using UnityEngine;

namespace Funes.Painting
{
    /// <summary>
    /// Core paint behaviour. Each frame it asks its <see cref="PaintProbe"/> for a
    /// ray + activation, raycasts onto a <see cref="PaintableSurface"/>, and stamps
    /// the brush at the hit's UV. Fully device-independent — swap the PaintProbe
    /// for VR / mouse / (future) touch without changing this class.
    /// </summary>
    [RequireComponent(typeof(PaintProbe))]
    public class PaintTool : MonoBehaviour
    {
        public enum Mode { Brush, Spray }

        [SerializeField] Mode mode = Mode.Spray;

        [Header("Brush shape (UV space)")]
        [SerializeField, Range(0.005f, 0.3f)] float brushRadius = 0.03f;
        [SerializeField, Range(0.05f, 0.5f)]  float sprayRadius = 0.12f;
        [SerializeField, Range(0f, 1f)] float brushHardness = 0.8f;
        [SerializeField, Range(0f, 1f)] float sprayHardness = 0.25f;

        [Header("Fill")]
        [Tooltip("Reveal amount added per second at full activation.")]
        [SerializeField] float fillRate = 3f;

        [Header("Reach")]
        [SerializeField] float maxDistance = 5f;
        [SerializeField] LayerMask paintableLayers = ~0;

        PaintProbe _probe;

        void Awake() => _probe = GetComponent<PaintProbe>();

        void Update()
        {
            if (!_probe.TryGetPaintRay(out Ray ray, out float activation)) return;

            if (!Physics.Raycast(ray, out RaycastHit hit, maxDistance, paintableLayers, QueryTriggerInteraction.Ignore))
                return;

            var surface = hit.collider.GetComponentInParent<PaintableSurface>();
            if (surface == null) return;

            // hit.textureCoord requires a non-convex MeshCollider on a readable mesh.
            Vector2 uv = hit.textureCoord;

            float radius   = mode == Mode.Spray ? sprayRadius : brushRadius;
            float hardness = mode == Mode.Spray ? sprayHardness : brushHardness;
            float strength = activation * fillRate * Time.deltaTime;

            surface.PaintAt(uv, radius, strength, hardness);
        }

        /// <summary>Switch between spray and brush at runtime (e.g. from a UI/tool swap).</summary>
        public void SetMode(Mode m) => mode = m;
    }
}
