Shader "Custom/CameraShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			
			#include "UnityCG.cginc"

			#pragma multi_compile __ UNITY_UI_ALPHACLIP

			struct appdata
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				float4 worldPos : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f vert (appdata v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				//o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.worldPos = v.vertex;
				o.vertex = UnityObjectToClipPos(v.vertex);
				//o.uv = v.uv;
				//o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.texcoord = v.texcoord;
				return o;
			}
			
			sampler2D _MainTex;
			float _AberrationOffset;
			float _EffectRange;
			float _EffectStrength;
			float _ResoX;
			float _ResoY;
			float4 _PlayerPos;

			fixed4 frag (v2f i) : COLOR
			{
				//float2 coords = i.w .xy;
				float2 playerPos = _PlayerPos.xy;
				//float2 playerPos = UnityObjectToClipPos(_PlayerPos).xy;
				float2 coords = i.worldPos.xy;
				_AberrationOffset /= 1.0f;
				float xDiff = abs(coords.x - playerPos.x);
				float yDiff = abs(coords.y - playerPos.y);
				//fixed4 col = tex2D(_MainTex, i.uv + float2(( i.vertex.x - 960)/10000, ( i.vertex.y - 540)/10000));
				//fixed4 col = tex2D(_MainTex, i.uv);
				//// just invert the colors
				////col = 1 - col;
				//return col;
				//Red Channel
	            float4 red = tex2D(_MainTex , coords.xy - _AberrationOffset * xDiff* yDiff)+ (xDiff + yDiff)/20;
	            //float4 red = tex2D(_MainTex , coords.xy ) + (xDiff + yDiff)/20;
	            //Green Channel
	            float4 green = tex2D(_MainTex, coords.xy );
	            //Blue Channel
	            float4 blue = tex2D(_MainTex, coords.xy );
	            //float4 blue = tex2D(_MainTex, coords.xy + _AberrationOffset/(coords.x+coords.y));
	           
	            float4 finalColor = float4(red.r, green.g, blue.b, 1.0f) * sqrt(1- xDiff) *sqrt(1 - yDiff);
	            return finalColor;
			}
			ENDCG
		}
	}
}
