Shader "Funes/RevealMask"
{
    // Renders a model that starts "uncolored" and is progressively revealed
    // toward its real albedo wherever the reveal mask has been painted.
    // Unlit + fragment-based on purpose: fast on Quest and WebGL2-safe
    // (no compute), per the PRD's web-portability constraint.
    Properties
    {
        [MainTexture] _BaseMap ("Target Color (Albedo)", 2D) = "white" {}
        _UncoloredColor ("Uncolored Color", Color) = (1,1,1,1)
        _RevealMask ("Reveal Mask (R)", 2D) = "black" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" "Queue"="Geometry" }

        Pass
        {
            Name "ForwardUnlit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes { float4 positionOS : POSITION; float2 uv : TEXCOORD0; };
            struct Varyings   { float4 positionHCS : SV_POSITION; float2 uv : TEXCOORD0; };

            TEXTURE2D(_BaseMap);    SAMPLER(sampler_BaseMap);
            TEXTURE2D(_RevealMask); SAMPLER(sampler_RevealMask);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _UncoloredColor;
            CBUFFER_END

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                half3 target = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv).rgb;
                half  mask   = SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, IN.uv).r;
                half3 col    = lerp(_UncoloredColor.rgb, target, saturate(mask));
                return half4(col, 1.0);
            }
            ENDHLSL
        }
    }
}
