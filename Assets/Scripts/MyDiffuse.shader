Shader "Custom/MyDiffuse"
{
	Properties
	{
		_Color( "Main Color", Color ) = ( 1,1,1,1 )
		_MainTex( "Base (RGB) Trans (A)", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "Queue" = "Geometry" "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert addshadow

		sampler2D _MainTex;
		fixed4 _Color;

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf( Input IN, inout SurfaceOutput o )
		{
			fixed4 c = tex2D( _MainTex, IN.uv_MainTex ) * _Color;
			o.Albedo = step( IN.uv_MainTex.x , 0.97 ) * step( IN.uv_MainTex.y , 0.97 ) * c.rgb;
			o.Alpha = c.a;
		}
		ENDCG

		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }

			ZWrite On ZTest LEqual

			CGPROGRAM
			#pragma target 3.0
			//#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma multi_compile_shadowcaster
			
			#pragma vertex vertShadowCaster
			#pragma fragment fragShadowCaster
			
			#include "UnityStandardShadow.cginc"
			
			ENDCG
		}
	}

	Fallback "Legacy Shaders/Transparent/VertexLit"
}
