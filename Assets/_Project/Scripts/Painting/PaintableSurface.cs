using UnityEngine;
using UnityEngine.Rendering;

namespace Funes.Painting
{
    /// <summary>
    /// Holds a render-texture "reveal mask" for one model and stamps brush strokes
    /// into it. Stamping renders the mesh's UV unwrap into the mask while testing
    /// WORLD distance from the brush point (Funes/BrushStampWorld), so brushes are
    /// sized in meters and never bleed across UV islands onto far-away geometry.
    /// The model's material (Funes/RevealMask) samples the mask to lerp from an
    /// uncolored color toward its real albedo.
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class PaintableSurface : MonoBehaviour
    {
        [Tooltip("Resolution of the square reveal mask render texture.")]
        [SerializeField] int maskResolution = 1024;

        [Tooltip("Stamp shader. Leave empty to auto-find Funes/BrushStampWorld.")]
        [SerializeField] Shader brushShader;

        RenderTexture _mask;
        Material _brushMat;
        Renderer _renderer;
        Mesh _mesh;
        MaterialPropertyBlock _props;
        CommandBuffer _cb;

        static readonly int RevealMaskID    = Shader.PropertyToID("_RevealMask");
        static readonly int RevealMaskTexelSizeID = Shader.PropertyToID("_RevealMask_TexelSize");
        static readonly int BrushWorldPosID = Shader.PropertyToID("_BrushWorldPos");
        static readonly int BrushRadiusID   = Shader.PropertyToID("_BrushRadius");
        static readonly int HardnessID      = Shader.PropertyToID("_Hardness");
        static readonly int StrengthID      = Shader.PropertyToID("_Strength");

        void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _props = new MaterialPropertyBlock();
            _cb = new CommandBuffer { name = "Funes Paint Stamp" };

            var mf = GetComponent<MeshFilter>();
            if (mf != null) _mesh = mf.sharedMesh;
            if (_mesh == null && _renderer is SkinnedMeshRenderer smr) _mesh = smr.sharedMesh;
            if (_mesh == null)
                Debug.LogError($"[PaintableSurface] No mesh found on '{name}'.", this);

            // Linear: the mask is data (reveal amount), not color.
            _mask = new RenderTexture(maskResolution, maskResolution, 0, RenderTextureFormat.R8, RenderTextureReadWrite.Linear)
            {
                wrapMode   = TextureWrapMode.Clamp,
                filterMode = FilterMode.Bilinear,
                name       = "RevealMask"
            };
            _mask.Create();
            ClearMask();

            if (brushShader == null) brushShader = Shader.Find("Funes/BrushStampWorld");
            _brushMat = new Material(brushShader);

            _renderer.GetPropertyBlock(_props);
            _props.SetTexture(RevealMaskID, _mask);
            // MPB-bound textures don't get _TexelSize auto-filled; the shader needs
            // it for seam dilation.
            _props.SetVector(RevealMaskTexelSizeID,
                new Vector4(1f / maskResolution, 1f / maskResolution, maskResolution, maskResolution));
            _renderer.SetPropertyBlock(_props);
        }

        /// <summary>
        /// Stamp a soft spherical brush around a world-space point. Radius is in
        /// meters; only surface within that 3D distance is painted, regardless of
        /// how the UV atlas is packed.
        /// </summary>
        public void PaintAt(Vector3 worldPos, float radiusMeters, float strength, float hardness = 0.5f)
        {
            if (_mesh == null) return;

            _brushMat.SetVector(BrushWorldPosID, worldPos);
            _brushMat.SetFloat(BrushRadiusID, radiusMeters);
            _brushMat.SetFloat(HardnessID, hardness);
            _brushMat.SetFloat(StrengthID, strength);

            _cb.Clear();
            _cb.SetRenderTarget(_mask);
            for (int sub = 0; sub < _mesh.subMeshCount; sub++)
                _cb.DrawMesh(_mesh, transform.localToWorldMatrix, _brushMat, sub, 0);
            Graphics.ExecuteCommandBuffer(_cb);
        }

        /// <summary>Wipe the model back to fully uncolored.</summary>
        public void ResetMask() => ClearMask();

        void ClearMask()
        {
            var prev = RenderTexture.active;
            RenderTexture.active = _mask;
            GL.Clear(false, true, Color.black); // black = fully uncolored
            RenderTexture.active = prev;
        }

        void OnDestroy()
        {
            if (_mask != null) _mask.Release();
            if (_brushMat != null) Destroy(_brushMat);
            _cb?.Release();
        }
    }
}
