Shader "Custom/FogDissolve"{
	Properties {
		_Color("Color", Color) = (1, 1, 1, 1)
		_MainTex("Texture", 2D) = "white" {}
		_NoiseTex("Noise Texture", 2D) = "gray" {}
		_DissolveThresholdMin("Dissolve Threshold (Min)", Range(0, 1)) = 0
		_DissolveThresholdMax("Dissolve Threshold (Max)", Range(0, 1)) = 0.3
	}

	SubShader{
		// Make sure we can do transparency.
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 100
		ZWrite Off                       // Don't obscure objects in the depth buffer.
		Blend SrcAlpha OneMinusSrcAlpha  // Use blend for alpha values.

		Pass {
			CGPROGRAM
			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmetProgram

			#include "UnityCG.cginc"

			struct VextexData {
				float4 position : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct VetexToFragment {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uvNoise : TEXCOORD1;
			};

			// Turn our properties into variables.
			float4 _Color;
			sampler2D _MainTex, _NoiseTex;
			float4 _MainTex_ST, _NoiseTex_ST;
			float _DissolveThresholdMin;
			float _DissolveThresholdMax;

			// Map the textures to the vertexes.
			VetexToFragment MyVertexProgram(VextexData vertex) {
				VetexToFragment v2f;

				v2f.position = UnityObjectToClipPos(vertex.position);
				v2f.uv = vertex.uv * _MainTex_ST.xy + _MainTex_ST.zw;
				v2f.uvNoise = vertex.uv * _NoiseTex_ST.xy + _NoiseTex_ST.zw;

				return v2f;
			}

			// Map the colors of each fragment.
			float4 MyFragmetProgram(VetexToFragment v2f) : SV_TARGET {
				// Get the colors of the textures.
				float4 colorValue = tex2D(_MainTex, v2f.uv);
				float4 noiseColorValue = tex2D(_NoiseTex, v2f.uvNoise);

				// Make the time sine oscillate from 0 to 1.
				float oscTime = (_SinTime.z + 1) / 2;

				// Apply the noise texture to create transparent spots.
				clip(noiseColorValue.rgb - (_DissolveThresholdMin + oscTime * _DissolveThresholdMax));

				// Apply a final color and also control the overall brightness.
				return colorValue * _Color;
			}
			ENDCG
		}
	}
}