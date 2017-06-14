// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Custom/Mobile/Diffuse" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 150

		CGPROGRAM
		#pragma surface surf Lambert noforwardadd

		sampler2D _MainTex;
		fixed4 _Color;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			//float3 _0 = UNITY_MATRIX_T_MV[0];
			//float3 _1 = UNITY_MATRIX_T_MV[1];
			//float x = IN.uv_MainTex.x * length(_0);
			//float y = IN.uv_MainTex.y * length(_1);
			//fixed4 c = tex2D(_MainTex, float2(x, y));
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			c = c * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}

	Fallback "Mobile/Diffuse"
}
