Shader "Unlit/Bars"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Size("Size", float) = 8.0
        _Speed("Wave Speed", float) = 4.0
        _LineThickness("Line Thickness", float) = 0.16
        _LineSmoothness("Line Smoothness", float) = 0.04
        _YOffset("Y Offset", float) = 0.0
        _YHeight("Y Height", float) = 4.0
        _PeakWidth("Peak Width", float) = 1.0
        _PeakSmoothness("Peak Smoothness", float) = 0.7
        _EdgeCutoff("Edge Cutoff", float) = 0.1
        _Color ("Color", Color) = (0.1,0.1,0.1,1)
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

            float distToLine(float2 A, float2 B, float2 p){
                float2 PA = p - A;
                float2 BA = B - A;
                float d = dot(PA,BA);
                float t = clamp(d/(length(BA)*length(BA)),0., 1.);//Vektorprodukt
                float2 normal = PA - BA*t;
                return length(normal);
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
            };

            float _Timer;

            sampler2D _MainTex;
            float4 _MainTex_ST;
            uniform fixed4 _Color;
            uniform float _Size;
            uniform float _Speed;
            uniform float _YOffset;
            uniform float _YHeight;
            uniform float _PeakSmoothness;
            uniform float _PeakWidth;
            uniform float _LineThickness;
            uniform float _LineSmoothness;
            uniform float _EdgeCutoff;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Normalized pixel coordinates (from -0.5,0.5)
                float2 uv = (i.uv * 2.0) - 1.0;
                uv.x -= 0.5;
                uv *= _Size;

                float xID = _PeakWidth * floor(uv.x);

                float y = _YOffset + _YHeight * smoothstep(0.0, _PeakSmoothness, sin(xID/_Size - _Timer * _Speed));

                uv = float2(frac(uv.x)-0.5, uv.y);

                //Line Points
                float2 A1 = float2(0.0, -y);
                float2 B1 = float2(0.0, y);

                //line
                fixed4 col = fixed4(0.0, 0.0, 0.0, 0.0);
                float f = 1.0 - smoothstep(_LineThickness - _LineSmoothness, _LineThickness, abs(distToLine(A1, B1, uv)));
                col = lerp(col, _Color, f);

                //Slice edges
                // bottom-left
                fixed2 bl = step(fixed2(_EdgeCutoff, _EdgeCutoff),i.uv);
                fixed pct = bl.x * bl.y;
                // top-right
                fixed2 tr = step(fixed2(_EdgeCutoff, _EdgeCutoff),1.0-i.uv);
                pct *= tr.x * tr.y;
                fixed border = 1.0 - pct;
                clip(-border);
                //col -= fixed4(border, border, border, border);

                //fixed4 col = tex2D(_MainTex, i.uv);
                //fixed4 col = fixed4(xID, 0.0, 0.0, 1.0);
                //return fixed4(.5, .5, .5, border);
                return col;
            }
            ENDCG
        }
    }
}
