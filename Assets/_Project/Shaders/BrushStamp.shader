Shader "Funes/BrushStamp"
{
    // Used by PaintableSurface via Graphics.Blit to additively stamp a soft
    // circular brush into the reveal mask (ping-ponged each call). Built-in
    // CG blit shader = the battle-tested, version-stable path for Graphics.Blit.
    Properties { _MainTex ("Mask", 2D) = "black" {} }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f     { float2 uv : TEXCOORD0; float4 vertex : SV_POSITION; };

            sampler2D _MainTex;
            float4 _Brush;   // xy = center UV, z = radius (UV), w = hardness 0..1
            float  _Strength;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed src   = tex2D(_MainTex, i.uv).r;
                float d     = distance(i.uv, _Brush.xy);
                float inner = _Brush.z * saturate(_Brush.w);
                fixed brush = 1.0 - smoothstep(inner, _Brush.z, d);
                fixed outv  = saturate(src + brush * _Strength);
                return fixed4(outv, outv, outv, 1.0);
            }
            ENDCG
        }
    }
}
