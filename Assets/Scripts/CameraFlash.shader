﻿Shader "Custom/CameraFlash"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color ("Flash Color", Color) = (1,1,1,1)
		_Progress("Control Flash", Range(0,1)) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

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

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float _Progress;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float halfPI = 1.570796;
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 flashCol = (1 - (1 - _Color) * sin(halfPI + _Progress * halfPI) * 0.5) + step(_Progress, 0);
				return col * min(1, flashCol);
			}
			ENDCG
		}
	}
}
