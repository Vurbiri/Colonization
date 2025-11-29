Shader "UI/SimpleGradient" 
{
	Properties 
	{
		_Color ("Start Color", Color) = (0, 0, 0, 1)
		_Color2 ("End Color", Color) = (1, 1, 1, 1)
		[Toggle] _IsVertical ("Is Vertical", Int) = 0
		[Space]
		[Hidden][PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		[Hidden] _StencilComp ("Stencil Comparison", Float) = 8
		[Hidden] _Stencil ("Stencil ID", Float) = 0
		[Hidden] _StencilOp ("Stencil Operation", Float) = 0
		[Hidden] _StencilWriteMask ("Stencil Write Mask", Float) = 255
		[Hidden] _StencilReadMask ("Stencil Read Mask", Float) = 255
		[Hidden] _ColorMask ("Color Mask", Float) = 15
		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
	}
 
	SubShader 
	{
		Tags
		{
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

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
 
		Pass 
		{
			Name "Default"
		CGPROGRAM
			#pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
						
			fixed4 _Color;
			fixed4 _Color2;
			int _IsVertical;
 
			struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
            };

			v2f vert(appdata_t v)
            {
                v2f OUT;

                OUT.vertex = UnityObjectToClipPos(v.vertex);
				OUT.color = v.color * lerp(_Color, _Color2, _IsVertical ? v.texcoord.y : v.texcoord.x);

                return OUT;
            }

            fixed4 frag(v2f IN) : COLOR
            {
                return IN.color;
            }

		ENDCG
		}
	}
}