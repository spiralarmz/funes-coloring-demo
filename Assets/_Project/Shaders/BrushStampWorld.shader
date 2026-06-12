Shader "Funes/BrushStampWorld"
{
    // Texture-space painting with world-space brushes: PaintableSurface draws the
    // paintable MESH with this shader, targeting the reveal-mask render texture.
    // The vertex stage places each vertex at its UV coordinate (we rasterize the
    // unwrap), while the fragment stage stamps by WORLD distance from the brush
    // hit point. Only texels whose 3D surface lies within the brush radius are
    // painted - UV islands belonging to far-away geometry (e.g. the model's back
    // side packed next to the front in the atlas) are untouched, and brush size
    // is true meters regardless of UV scale.
    SubShader
    {
        Pass
        {
            Cull Off
            ZWrite Off
            ZTest Always
            Blend One One   // additive accumulation; R8 target clamps at 1

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f     { float4 pos : SV_POSITION; float3 worldPos : TEXCOORD0; };

            float3 _BrushWorldPos;
            float  _BrushRadius;   // meters
            float  _Hardness;      // 0..1, fraction of radius that is full strength
            float  _Strength;

            v2f vert (appdata v)
            {
                v2f o;
                // Rasterize the mesh's UV unwrap: UV (0..1) -> clip space (-1..1).
                float2 uvClip = v.uv * 2.0 - 1.0;
                #if UNITY_UV_STARTS_AT_TOP
                    uvClip.y = -uvClip.y;
                #endif
                o.pos = float4(uvClip, 0.0, 1.0);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float d     = distance(i.worldPos, _BrushWorldPos);
                float inner = _BrushRadius * saturate(_Hardness);
                float brush = 1.0 - smoothstep(inner, _BrushRadius, d);
                float outv  = brush * _Strength;
                return fixed4(outv, outv, outv, 1.0);
            }
            ENDCG
        }
    }
}
