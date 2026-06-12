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

        [Header("Brush shape (world space, meters)")]
        [SerializeField, Range(0.01f, 1f)] float brushRadiusMeters = 0.05f;
        [SerializeField, Range(0.05f, 3f)] float sprayRadiusMeters = 0.25f;
        [SerializeField, Range(0f, 1f)] float brushHardness = 0.8f;
        [SerializeField, Range(0f, 1f)] float sprayHardness = 0.25f;

        [Header("Fill")]
        [Tooltip("Reveal amount added per second at full activation.")]
        [SerializeField] float fillRate = 3f;

        [Header("Reach")]
        [SerializeField] float maxDistance = 5f;
        [SerializeField] LayerMask paintableLayers = ~0;

        [Header("Aim aid (optional)")]
        [Tooltip("If assigned, draws a line from the tool to where the spray will land while active.")]
        [SerializeField] LineRenderer aimLine;

        PaintProbe _probe;

        void Awake()
        {
            _probe = GetComponent<PaintProbe>();
            if (aimLine != null) aimLine.enabled = false;
        }

        void Update()
        {
            // Aim pose is available whenever the tool has one (held tool: always);
            // painting additionally requires activation (trigger past threshold).
            bool painting = _probe.TryGetPaintRay(out Ray ray, out float activation);
            if (!painting && !_probe.TryGetAimRay(out ray))
            {
                if (aimLine != null) aimLine.enabled = false;
                return;
            }

            bool didHit = Physics.Raycast(ray, out RaycastHit hit, maxDistance, paintableLayers, QueryTriggerInteraction.Ignore);
            UpdateAimLine(ray, didHit, hit);
            if (!painting || !didHit) return;

            var surface = hit.collider.GetComponentInParent<PaintableSurface>();
            if (surface == null) return;

            float radius   = mode == Mode.Spray ? sprayRadiusMeters : brushRadiusMeters;
            float hardness = mode == Mode.Spray ? sprayHardness : brushHardness;
            float strength = activation * fillRate * Time.deltaTime;

            surface.PaintAt(hit.point, radius, strength, hardness);
            _probe.NotifyPaintApplied(activation);
        }

        void UpdateAimLine(Ray ray, bool didHit, RaycastHit hit)
        {
            if (aimLine == null) return;
            aimLine.enabled = true;
            aimLine.positionCount = 2;
            aimLine.SetPosition(0, ray.origin);
            aimLine.SetPosition(1, didHit ? hit.point : ray.origin + ray.direction * maxDistance);
        }

        /// <summary>Switch between spray and brush at runtime (e.g. from a UI/tool swap).</summary>
        public void SetMode(Mode m) => mode = m;
    }
}
