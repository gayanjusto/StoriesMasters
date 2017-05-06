Shader "Sprites/Outline"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
	_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0

		_OutlineSize("Outline Size", int) = 1

		// Add values to determine if outlining is enabled and outline color.
		[PerRendererData] _Outline("Outline", Float) = 1
		[PerRendererData] _OutlineColor("Outline Color", Color) = (0,1,0,1)
		_BaseColorTex("Base (RGB)", 2D) = "white" {}
		_BaseColorAlpha("Alpha (A)", 2D) = "white" {}

	
	}

		SubShader
	{
		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile _ PIXELSNAP_ON
#pragma shader_feature ETC1_EXTERNAL_ALPHA
#include "UnityCG.cginc"

		struct appdata_t
	{
		float4 vertex   : POSITION;
		float4 color    : COLOR;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f
	{
		float4 vertex   : SV_POSITION;
		fixed4 color : COLOR;
		float2 texcoord  : TEXCOORD0;
	};

	fixed4 _Color;
	float _Outline;
	fixed4 _OutlineColor;
	int _OutlineSize;

	v2f vert(appdata_t IN)
	{
		v2f OUT;
		OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
		OUT.texcoord = IN.texcoord;
		OUT.color = IN.color * _Color;
#ifdef PIXELSNAP_ON
		OUT.vertex = UnityPixelSnap(OUT.vertex);
#endif

		return OUT;
	}

	sampler2D _MainTex;
	sampler2D _AlphaTex;
	sampler2D _BaseColorTex;
	sampler2D _BaseColorAlpha;
	float4 _MainTex_TexelSize;

	fixed4 SampleSpriteTexture(float2 uv)
	{
		fixed4 color = tex2D(_BaseColorTex, uv);

#if ETC1_EXTERNAL_ALPHA
		// get the color from an external texture (usecase: Alpha support for ETC1 on android)
		color.a = tex2D(_AlphaTex, uv).r;
#endif //ETC1_EXTERNAL_ALPHA

		return color;
	}

	//Runs for every pixel
	fixed4 frag(v2f IN) : SV_Target
	{
		fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;

	// If outline is enabled and there is a pixel, try to draw an outline.
	if (/*_Outline > 0 &&*/ c.a != 0) {

		// Get the neighbouring four pixels.
		fixed4 pixelUp = tex2D(_MainTex, IN.texcoord + fixed2(0, _MainTex_TexelSize.y));
		fixed4 pixelDown = tex2D(_MainTex, IN.texcoord - fixed2(0, _MainTex_TexelSize.y));
		fixed4 pixelRight = tex2D(_MainTex, IN.texcoord + fixed2(_MainTex_TexelSize.x, 0));
		fixed4 pixelLeft = tex2D(_MainTex, IN.texcoord - fixed2(_MainTex_TexelSize.x, 0));
		fixed4 currentPixel = tex2D(_MainTex, IN.texcoord);

		//Para contornar apenas os pixels transparentes proximos da textura
		//devemos verificar se o atual é transparente ou não

		// If current pixel is visible but neighbours aren't, we must highlight them
		if ((pixelUp.a * pixelDown.a * pixelRight.a * pixelLeft.a == 0 && currentPixel.a != 0)	) {
			c.rgba = fixed4(1, 1, 1, 1) * fixed4(0, 0, 0, 1);
		}

		//If all neighbouring pixels are invisible, we must hide them
		else if (pixelUp.a * pixelDown.a * pixelRight.a * pixelLeft.a == 0){
			c.rgba = fixed4(0, 0, 0, 0) ;
		}
	}

	c.rgb *= c.a;

	return c;
	}
		ENDCG
	}
	}
}