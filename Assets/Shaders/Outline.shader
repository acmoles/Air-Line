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
            #pragma multi_compile_instancing
            #pragma instancing_options assumeuniformscaling

            #include "UnityCG.cginc"

            struct appdata
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : SV_POSITION;
                float3 worldNormal : NORMAL;
                float3 viewDir : TEXCOORD1;
            };
      
            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float, _GlobalAlpha)
            UNITY_INSTANCING_BUFFER_END(Props)

            uniform float4 _Color;
            uniform float _RimAmount;
            uniform float _RimSmoothness;
            uniform float _RimThickness;

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = WorldSpaceViewDir(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                fixed4 col = _Color;

                float3 normal = normalize(i.worldNormal);
                float3 viewDir = normalize(i.viewDir);

                float4 rimDot = 1 - abs(dot(viewDir, normal));

                float rimInner = smoothstep(_RimAmount - _RimThickness - _RimSmoothness, _RimAmount - _RimThickness, rimDot);
                float rimOuter = smoothstep(_RimAmount + _RimThickness, _RimAmount + _RimThickness + _RimSmoothness, rimDot);

                float rimIntensity = rimInner - rimOuter;
                float innerIntensity = saturate(col.a - rimInner) * UNITY_ACCESS_INSTANCED_PROP(Props, _GlobalAlpha);
                float combinedAlpha = rimIntensity * UNITY_ACCESS_INSTANCED_PROP(Props, _GlobalAlpha) + innerIntensity;

                float4 output = float4(col.rgb, combinedAlpha);
                return output;
            }
            ENDCG
        }
    }
}
