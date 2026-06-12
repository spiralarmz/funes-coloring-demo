Shader "Funes/RevealMask"
{
    // Renders a model that starts "uncolored" and is progressively revealed
    // toward its real albedo wherever the reveal mask has been painted.
    // Unlit + fragment-based on purpose: fast on Quest and WebGL2-safe
    // (no compute), per the PRD's web-portability constraint.
    //
    // Stereo: includes single-pass-instanced macros — required on Quest/Link
    // (without them the object renders in the left eye only).
    // Seams: the mask is sampled with a 5-tap max "dilation" so painted area
    // bleeds one texel outward, hiding white cracks along UV island borders.
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
            #pragma multi_compile_instancing
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_BaseMap);    SAMPLER(sampler_BaseMap);
            TEXTURE2D(_RevealMask); SAMPLER(sampler_RevealMask);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _UncoloredColor;
                float4 _RevealMask_TexelSize;
            CBUFFER_END

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half SampleMaskDilated (float2 uv)
            {
                // Max of center + 4 neighbors: painted area grows ~1 texel past
                // UV island borders, so bilinear blending toward unpainted gutter
                // texels no longer draws white cracks along the seams.
                float2 t = _RevealMask_TexelSize.xy;
                half m = SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, uv).r;
                m = max(m, SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, uv + float2( t.x, 0)).r);
                m = max(m, SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, uv + float2(-t.x, 0)).r);
                m = max(m, SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, uv + float2(0,  t.y)).r);
                m = max(m, SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, uv + float2(0, -t.y)).r);
                return m;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

                half3 target = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv).rgb;
                half  mask   = SampleMaskDilated(IN.uv);
                half3 col    = lerp(_UncoloredColor.rgb, target, saturate(mask));
                return half4(col, 1.0);
            }
            ENDHLSL
        }
    }
}
