Shader "Custom/SpriteGradient" {
Properties {
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    _Color ("Tint", Color) = (1,1,1,1)
    _Color1 ("Left Color", Color) = (1,1,1,1)
    _Color2 ("Right Color", Color) = (1,1,1,1)
    _Scale ("Scale", Float) = 1

    _StencilComp ("Stencil Comparison", Float) = 8
    _Stencil ("Stencil ID", Float) = 0
    _StencilOp ("Stencil Operation", Float) = 0
    _StencilWriteMask ("Stencil Write Mask", Float) = 255
    _StencilReadMask ("Stencil Read Mask", Float) = 255

    _ColorMask ("Color Mask", Float) = 15

    [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
}

SubShader {
    Tags {
        "Queue"="Transparent"
        "IgnoreProjector"="True"
        "RenderType"="Transparent"
        "PreviewType"="Plane"
        "CanUseSpriteAtlas"="True"
    }
    LOD 100

    Stencil
    {
        Ref [_Stencil]
        Comp [_StencilComp]
        Pass [_StencilOp]
        ReadMask [_StencilReadMask]
        WriteMask [_StencilWriteMask]
    }

    Cull Off
    Lighting Off
    ZWrite Off
    ZTest [unity_GUIZTestMode]
    Blend SrcAlpha OneMinusSrcAlpha
    ColorMask [_ColorMask]

    Pass {
    CGPROGRAM
    #pragma vertex vert  
    #pragma fragment frag
    #pragma target 2.0

    #include "UnityCG.cginc"
    #include "UnityUI.cginc"

    fixed4 _Color;
    fixed4 _Color1;
    fixed4 _Color2;
    fixed  _Scale;
    sampler2D _MainTex;
    fixed4 _TextureSampleAdd;
    float4 _MainTex_ST;

    #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
    #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

    struct appdata_t
    {
        float4 vertex   : POSITION;
        float4 color    : COLOR;
        float2 texcoord : TEXCOORD0;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct v2f {
        float4 position : SV_POSITION;
        fixed4 color : COLOR;
        float2 texcoord  : TEXCOORD0;
        UNITY_VERTEX_OUTPUT_STEREO
    };

    v2f vert (appdata_t v)
    {
        v2f o;
        UNITY_SETUP_INSTANCE_ID(v);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
        o.position = UnityObjectToClipPos (v.vertex);
        o.color = lerp(_Color2,_Color1, v.texcoord.y ) * _Color;
        o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
        return o;
    }

    float4 frag (v2f i) : COLOR {
        half4 color = (tex2D(_MainTex, i.texcoord) + _TextureSampleAdd) * i.color;
        //color.rgb *= color.a;

        #ifdef UNITY_UI_ALPHACLIP
        clip (color.a - 0.001);
        #endif

        return color;
    }
    ENDCG
    }
    }
}