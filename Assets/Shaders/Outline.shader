Shader "Unlit/Outline"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
        _RimAmount("Rim Amount", Range(0, 1)) = 0.716
        _RimSmoothness("Rim Smoothness", Range(0.01, 0.1)) = 0.05
        _RimThickness("Rim Thickness", Range(0, 1)) = 0.1
        _GlobalAlpha("Alpha", float) = 1.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Cull Off
            ZWrite Off
            Blend SrcAlpha One // additive
            //OneMinusSrcAlpha // standard alpha blending

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldNormal : NORMAL;
                float3 viewDir : TEXCOORD1;
            };

            uniform float4 _Color;
            uniform float _RimAmount;
            uniform float _RimSmoothness;
            uniform float _RimThickness;
            uniform float _GlobalAlpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = WorldSpaceViewDir(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = _Color;

                float3 normal = normalize(i.worldNormal);
                float3 viewDir = normalize(i.viewDir);

                float4 rimDot = 1 - abs(dot(viewDir, normal));

                float rimIntensity = smoothstep(_RimAmount - _RimThickness - _RimSmoothness, _RimAmount - _RimThickness, rimDot) - smoothstep(_RimAmount + _RimThickness, _RimAmount + _RimThickness + _RimSmoothness, rimDot);
                //TODO smoothstep minus smoothstep

                float4 output = float4(col.rgb, rimIntensity * _GlobalAlpha);
                return output;
                //return col;
            }
            ENDCG
        }
    }
}
