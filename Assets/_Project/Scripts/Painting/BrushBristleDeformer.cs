using UnityEngine;

namespace Funes.Painting
{
    /// <summary>
    /// Cozy brush feel: squashes and leans the bristles when they press into a
    /// surface. Self-contained — casts a short ray from the bristle base along its
    /// +Z and, on contact, (a) shortens the bristles so the tip sits on the surface
    /// instead of poking through it (compression), (b) leans them toward the surface
    /// plane (splay), and (c) optionally widens them a touch. Eases back to rest when
    /// lifted. Transform-only, so it's effectively free on Quest.
    ///
    /// Setup: assign <see cref="bristles"/> to a transform whose origin is at the
    /// BASE of the bristles with its local +Z pointing toward the tip (same convention
    /// as the ContactPaintProbe tip). The bristle mesh must be that transform (or a
    /// child of it) so scaling it shortens the bristles toward the base.
    /// </summary>
    public class BrushBristleDeformer : MonoBehaviour
    {
        [Tooltip("Bristle pivot: origin at the bristle base, local +Z toward the tip.")]
        [SerializeField] Transform bristles;

        [Tooltip("Bristle length in meters (how far past the base the tips reach).")]
        [SerializeField] float restLength = 0.03f;

        [Header("Deformation")]
        [Tooltip("Shortest the bristles compress to, as a fraction of rest length.")]
        [SerializeField, Range(0.1f, 1f)] float minSquash = 0.4f;
        [Tooltip("How far the bristles lean toward the surface plane at full press (0..1).")]
        [SerializeField, Range(0f, 1f)] float maxLean = 0.6f;
        [Tooltip("Sideways widening (fan) at full press, as a fraction.")]
        [SerializeField, Range(0f, 0.5f)] float splay = 0.15f;

        [Header("Feel")]
        [Tooltip("Higher = snappier give and recovery.")]
        [SerializeField] float responsiveness = 14f;
        [SerializeField] LayerMask contactLayers = ~0;

        Quaternion _restLocalRot;
        Vector3 _restLocalScale;
        Vector3 _restDirLocal;

        void Awake()
        {
            if (bristles == null) { enabled = false; return; }
            _restLocalRot   = bristles.localRotation;
            _restLocalScale = bristles.localScale;
            _restDirLocal   = _restLocalRot * Vector3.forward;
        }

        void LateUpdate()
        {
            Quaternion targetRot = _restLocalRot;
            Vector3 targetScale  = _restLocalScale;

            if (Physics.Raycast(bristles.position, bristles.forward, out RaycastHit hit,
                                 restLength, contactLayers, QueryTriggerInteraction.Ignore))
            {
                float squash = Mathf.Clamp(hit.distance / restLength, minSquash, 1f);
                float press  = 1f - Mathf.Clamp01(hit.distance / restLength); // 0 at touch .. 1 at full press

                // Compress along length; widen slightly to fan out.
                targetScale = new Vector3(
                    _restLocalScale.x * (1f + splay * press),
                    _restLocalScale.y * (1f + splay * press),
                    _restLocalScale.z * squash);

                // Lean toward the surface plane (bristles bend, not punch through).
                Vector3 tangent = Vector3.ProjectOnPlane(bristles.forward, hit.normal);
                if (tangent.sqrMagnitude > 1e-5f)
                {
                    Vector3 leanedWorld = Vector3.Slerp(bristles.forward, tangent.normalized, press * maxLean);
                    Vector3 leanedLocal = bristles.parent != null
                        ? bristles.parent.InverseTransformDirection(leanedWorld)
                        : leanedWorld;
                    targetRot = Quaternion.FromToRotation(_restDirLocal, leanedLocal) * _restLocalRot;
                }
            }

            // Frame-rate-independent easing toward target (gives the soft give + recovery).
            float t = 1f - Mathf.Exp(-responsiveness * Time.deltaTime);
            bristles.localRotation = Quaternion.Slerp(bristles.localRotation, targetRot, t);
            bristles.localScale    = Vector3.Lerp(bristles.localScale, targetScale, t);
        }
    }
}
