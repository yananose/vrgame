// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Cell" {
	Properties
	{
		[NoScaleOffset] _MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,0.95,0.9,1)
	}
	SubShader
	{
		Pass
		{
			// indicate that our pass is the "base" pass in forward
			// rendering pipeline. It gets ambient and main directional
			// light data set up; light direction in _WorldSpaceLightPos0
			// and color in _LightColor0
			Tags{ "LightMode" = "ForwardBase" }

			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc" // for UnityObjectToWorldNormal

			struct v2f
			{
				float2 uv : TEXCOORD0;
				fixed4 diff : COLOR0; // diffuse lighting color
				float4 vertex : SV_POSITION;
			};

			fixed4 _Color;

			v2f vert(appdata_base v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				half br = dot(worldNormal, _WorldSpaceLightPos0.xyz) + 1;
				o.diff = _Color;
				o.diff.a *= br * 0.5;
				return o;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				if (i.diff.a < 0.5) {
					col *= i.diff;
				}

				return col;
			}

			ENDCG
		}
	}

	Fallback "Unlit/Texture"
}