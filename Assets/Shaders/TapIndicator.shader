Shader "Unlit/TapIndicator"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Border("Border Width", float) = 0.2
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" }
        LOD 100
        Cull Off
        Lighting Off
        ZWrite Off
        ZTest Always
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            float circle(in float2 _st, in float _radius){
                float2 dist = _st - float2(0.5, 0.5);
                return 1.0 - smoothstep(_radius - (_radius * 0.01),
                                    _radius + (_radius * 0.01),
                                    dot(dist, dist) * 4.0);
            }

            struct appdata
            {
                float4 vertexColor: COLOR; // Vertex color
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertexColor: COLOR; // Vertex color
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            uniform float _Border;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertexColor = v.vertexColor;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 st = i.uv * 2.0 - 1.; // -1 to 1

                float d = length( abs(st) );

                fixed4 col = fixed4(0.0, 0.0, 0.0, 0.0);
                float ring = smoothstep(1.0 - _Border - 0.03, 1.0 - _Border, d) * smoothstep(0.99, 0.96, d);
                col = lerp(col, i.vertexColor, ring);
                return col;
                //return fixed4(d, d, d, d); 
            }
            ENDCG
        }
    }
}
