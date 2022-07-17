Shader "Unlit/ColorIndicator"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Border("Border Width", float) = 0.2
        _Color ("Color", Color) = (0.1,0.1,0.1,1)
        _ColorSecondary ("Color Secondary", Color) = (0.1,0.1,0.1,1)
        _ColorSecondaryAttenuation ("Color Secondary Attenuation", float) = 0.5
        _ColorBackground ("Color Background", Color) = (0.1,0.1,0.1,1)

        _LightAngle("Light Angle", float) = 1.5

        _Diffuse ("Diffuse", Range(0,1)) = 0.5
        _Glossiness ("Glossiness", Range(0,10)) = 0.5
        [HDR]
        _SpecularColor ("Specular Color", Color) = (1, 1, 1, 1)
        _SpecularAmount ("SpecularAmount", Range(0,10)) = 100
        [HDR]
        _RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
        _RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
        _RimThreshold("Rim Threshold", Range(0, 1)) = 0.1
        _NoiseAmount ("NoiseAmount", Range(0,1)) = 0.0
        _NoiseScale ("NoiseScale", float) = 1000.0
        _DarknessAmount ("DarknessAmount", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" }
        LOD 100
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "SimplexNoise2D.hlsl"
            #include "BlendModes.hlsl"

            float circle(in float2 _st, in float _radius){
                float2 dist = _st - float2(0.5, 0.5);
                return 1.0 - smoothstep(_radius - (_radius * 0.01),
                                    _radius + (_radius * 0.01),
                                    dot(dist, dist) * 4.0);
            }

            float circularGradient(float2 samplePosition, float radius){
                return length(samplePosition) - radius;
            }

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 screenPosition : TEXCOORD1;
            };

            float _Timer;

            sampler2D _MainTex;
            float4 _MainTex_ST;
            uniform fixed4 _Color;
            uniform fixed4 _ColorSecondary;
            uniform float _ColorSecondaryAttenuation;
            uniform fixed4 _ColorBackground;
            uniform float _Border;

            uniform float _LightAngle;

            uniform float _Diffuse;
            uniform float _Glossiness;
            uniform fixed4 _SpecularColor;
            uniform float _SpecularAmount;
            uniform float _RimPower;
            uniform float _RimThreshold;
            uniform float _NoiseAmount;
            uniform float _NoiseScale;
            uniform float _DarknessAmount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPosition = ComputeScreenPos(o.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 st = i.uv * 2.0 - 1.; // -1 to 1

                float d = length( abs(st) );

                // transform coordinates
                float c = cos(_LightAngle); 
                float s = sin(_LightAngle);
                float2x2 trans = { c, s, -s, c };  
                float2 sta = mul(trans, i.uv);

                // Diffuse
                fixed4 secondaryAttenuated = lerp(_Color, _ColorSecondary, _ColorSecondaryAttenuation);
                fixed4 comp = lerp(_Color, secondaryAttenuated, i.uv.y);
                comp = comp - _DarknessAmount*(1 - min(comp, sta.y));

                // Noise
                float2 stn = (i.screenPosition.xy / i.screenPosition.w) + frac(_Time);
                stn *= float2(_NoiseScale, _NoiseScale);
                float n = _NoiseAmount * SimplexNoise( stn );
                float3 noise = float3(n, n, n);
                comp.rgb = comp.rgb + (noise - .5 * noise);

                // Fresnel
                float df = pow(d, _RimPower) * 1.5;

                // Specular
                float2 stc = ((i.uv * 2.0 - 1.0) / 2.0) - 0.2;
                float dt = 1.0 - length( abs(stc) );
                dt = pow(dt, _Glossiness) * _SpecularAmount;

                // Blends
                dt += sta.y * _Diffuse;
                dt += smoothstep(_RimThreshold, 1.0, sta.y) * _Diffuse;
                dt += df * 0.1;
                comp.rgb = blendScreen(comp.rgb, dt);
                df += sta.y * _Diffuse;
                comp.rgb = blendColorDodge(comp.rgb, df);

                //Compositing the rings
                fixed4 col = fixed4(0.0, 0.0, 0.0, 0.0);
                col = lerp(col, comp, circle(i.uv, 1.0 - _Border / 2.0));
                float ring = smoothstep(1.0 - _Border - 0.03, 1.0 - _Border, d) * smoothstep(0.99, 0.96, d);
                col = lerp(col, _ColorBackground, ring);

                //return fixed4(center, 0.0, 1.0);
                //return fixed4(d, d, d, 1.0);
                return col; 
            }
            ENDCG
        }
    }
}
