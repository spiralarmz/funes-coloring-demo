using UnityEngine;

namespace Funes.Painting
{
    /// <summary>
    /// Holds a render-texture "reveal mask" for one model and stamps brush
    /// strokes into it in UV (texture) space. The model's material (Funes/RevealMask)
    /// samples the mask to lerp from an uncolored color toward its real albedo.
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class PaintableSurface : MonoBehaviour
    {
        [Tooltip("Resolution of the square reveal mask render texture.")]
        [SerializeField] int maskResolution = 1024;

        [Tooltip("Blit shader used to stamp brush strokes. Leave empty to auto-find Funes/BrushStamp.")]
        [SerializeField] Shader brushShader;

        RenderTexture _maskFront, _maskBack;
        Material _brushMat;
        Renderer _renderer;
        MaterialPropertyBlock _props;

        static readonly int RevealMaskID = Shader.PropertyToID("_RevealMask");
        static readonly int BrushID      = Shader.PropertyToID("_Brush");
        static readonly int StrengthID   = Shader.PropertyToID("_Strength");

        void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _props = new MaterialPropertyBlock();

            _maskFront = CreateMask();
            _maskBack  = CreateMask();
            Clear(_maskFront);
            Clear(_maskBack);

            if (brushShader == null) brushShader = Shader.Find("Funes/BrushStamp");
            _brushMat = new Material(brushShader);

            _renderer.GetPropertyBlock(_props);
            _props.SetTexture(RevealMaskID, _maskFront);
            _renderer.SetPropertyBlock(_props);
        }

        RenderTexture CreateMask()
        {
            // Linear read/write: the mask is data (reveal amount), not color, and
            // forcing linear avoids the unsupported R8_SRGB fallback in Linear projects.
            var rt = new RenderTexture(maskResolution, maskResolution, 0, RenderTextureFormat.R8, RenderTextureReadWrite.Linear)
            {
                wrapMode   = TextureWrapMode.Clamp,
                filterMode = FilterMode.Bilinear,
                name       = "RevealMask"
            };
            rt.Create();
            return rt;
        }

        static void Clear(RenderTexture rt)
        {
            var prev = RenderTexture.active;
            RenderTexture.active = rt;
            GL.Clear(false, true, Color.black); // black = fully uncolored
            RenderTexture.active = prev;
        }

        /// <summary>
        /// Stamp a soft circular brush at a UV coordinate (0..1). radius/hardness
        /// are in UV space; strength accumulates the mask toward a full reveal.
        /// </summary>
        public void PaintAt(Vector2 uv, float radius, float strength, float hardness = 0.5f)
        {
            _brushMat.SetVector(BrushID, new Vector4(uv.x, uv.y, radius, hardness));
            _brushMat.SetFloat(StrengthID, strength);

            Graphics.Blit(_maskFront, _maskBack, _brushMat);
            (_maskFront, _maskBack) = (_maskBack, _maskFront);

            _props.SetTexture(RevealMaskID, _maskFront);
            _renderer.SetPropertyBlock(_props);
        }

        /// <summary>Wipe the model back to fully uncolored.</summary>
        public void ResetMask()
        {
            Clear(_maskFront);
            Clear(_maskBack);
            _props.SetTexture(RevealMaskID, _maskFront);
            _renderer.SetPropertyBlock(_props);
        }

        void OnDestroy()
        {
            if (_maskFront != null) _maskFront.Release();
            if (_maskBack != null)  _maskBack.Release();
            if (_brushMat != null)  Destroy(_brushMat);
        }
    }
}
