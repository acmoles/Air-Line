Shader "Custom/VertexColored"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Cutout("Cutout", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
        _RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
        _NoiseAmount ("NoiseAmount", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        struct Input
        {
            float4 color: Color; // Vertex color
            float3 viewDir;
            float4 screenPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _Cutout;
        float _RimPower;
        float _NoiseAmount;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
            UNITY_DEFINE_INSTANCED_PROP(float4, _RimColor)
        UNITY_INSTANCING_BUFFER_END(Props)

        float noise(float2 co)
        {
            float a = 12.9898;
            float b = 78.233;
            float c = 43758.5453;
            float dt= dot(co.xy ,float2(a,b));
            float sn= fmod(dt,3.14);
            return frac(sin(sn) * c);
        }

        float3 blendOverlay(float3 base, float3 blend) {
            return lerp(
                sqrt(base) * (2.0 * blend - 1.0) + 2.0 * base * (1.0 - blend),
                2.0 * base * blend + base * base * (1.0 - 2.0 * blend),
                step(base, float3(0.5, 0.5, 0.5))
            );
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a vertex color or _Color if below alpha cutout threshhold
            fixed4 c = IN.color;
            if (all(IN.color > _Cutout)) { c = _Color; }
            o.Albedo = c.rgb;

            half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
            o.Emission = UNITY_ACCESS_INSTANCED_PROP(Props, _RimColor).rgb * pow (rim, _RimPower);

            float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
            screenUV *= float2(8,6);
            float n = 1.0 - _NoiseAmount * noise( screenUV );
            o.Albedo = blendOverlay(o.Albedo, float3(n, n, n));

            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
