Shader "Custom/VertexColored2"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Diffuse ("Diffuse", Range(0,1)) = 0.5
        _Glossiness ("Glossiness", Range(0,1)) = 0.5
        _SpecularAmount ("SpecularAmount", Range(0,2)) = 1.0
        _RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
        _RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
        _NoiseAmount ("NoiseAmount", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Opaque"
            "LightMode" = "ForwardBase"
            "PassFlags" = "OnlyDirectional"
        }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            struct appdata
            {
                float4 vertexColor: COLOR; // Vertex color
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertexColor: COLOR; // Vertex color
                float4 vertex : SV_POSITION;
                float3 worldNormal : NORMAL;
                float4 screenPosition : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
            };

            uniform fixed4 _Color;
            uniform float _Diffuse;
            uniform float _Glossiness;
            uniform float _SpecularAmount;
            uniform float _RimPower;
            uniform float _NoiseAmount;

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

            float3 blendOverlay(float3 base, float3 blend)
            {
                return lerp(
                    sqrt(base) * (2.0 * blend - 1.0) + 2.0 * base * (1.0 - blend),
                    2.0 * base * blend + base * base * (1.0 - 2.0 * blend),
                    step(base, float3(0.5, 0.5, 0.5))
                );
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = WorldSpaceViewDir(v.vertex);
                o.screenPosition = ComputeScreenPos(o.vertex);
                o.vertexColor = v.vertexColor;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Albedo comes from a vertex color or _Color if below alpha cutout threshhold
                fixed4 col = i.vertexColor;

                // Lighting
                float3 normal = normalize(i.worldNormal);
                float NdotL = dot(_WorldSpaceLightPos0, normal);
                
                // Specular
                float3 viewDir = normalize(i.viewDir);
                float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
                float NdotH = dot(normal, halfVector);
                float specularIntensity = pow(NdotH * _SpecularAmount, _Glossiness * _Glossiness);
                //float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);

                // Rim lighting
                half rim = 1.0 - saturate(dot (viewDir, normal));
                rim = UNITY_ACCESS_INSTANCED_PROP(Props, _RimColor).rgb * pow (rim, _RimPower);

                // Noise
                float2 screenUV = i.screenPosition.xy / i.screenPosition.w;
                screenUV *= float2(8,6);
                float n = 0.5 - _NoiseAmount * noise( screenUV );
                
                float4 light =  (_Diffuse * NdotL + n) * _LightColor0;

                //col.rgb = blendOverlay(col.rgb, light.rgb);
                //col = col + (specularIntensitySmooth);

                return float4(specularIntensity, specularIntensity, specularIntensity, specularIntensity);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
