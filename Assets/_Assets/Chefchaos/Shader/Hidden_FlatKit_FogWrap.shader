Shader "Hidden/FlatKit/FogWrap" {
	Properties {
		_Near ("Near", Float) = 0
		_Far ("Far", Float) = 100
		_DistanceFogIntensity ("Distance Fog Intensity", Range(0, 1)) = 1
		_LowWorldY ("Low", Float) = 0
		_HighWorldY ("High", Range(0, 1)) = 0.25
		_HeightFogIntensity ("Height Fog Intensity", Range(0, 1)) = 1
		_DistanceHeightBlend ("Distance / Height Blend", Range(0, 1)) = 0.5
		[NoScaleOffset] _DistanceLUT ("Distance LUT", 2D) = "white" {}
		[NoScaleOffset] _HeightLUT ("Height LUT", 2D) = "white" {}
		[HideInInspector] [NoScaleOffset] unity_Lightmaps ("unity_Lightmaps", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_LightmapsInd ("unity_LightmapsInd", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_ShadowMasks ("unity_ShadowMasks", 2DArray) = "" {}
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4x4 unity_ObjectToWorld;
			float4x4 unity_MatrixVP;

			struct Vertex_Stage_Input
			{
				float4 pos : POSITION;
			};

			struct Vertex_Stage_Output
			{
				float4 pos : SV_POSITION;
			};

			Vertex_Stage_Output vert(Vertex_Stage_Input input)
			{
				Vertex_Stage_Output output;
				output.pos = mul(unity_MatrixVP, mul(unity_ObjectToWorld, input.pos));
				return output;
			}

			float4 frag(Vertex_Stage_Output input) : SV_TARGET
			{
				return float4(1.0, 1.0, 1.0, 1.0); // RGBA
			}

			ENDHLSL
		}
	}
	Fallback "Hidden/Shader Graph/FallbackError"
	//CustomEditor "UnityEditor.Rendering.Fullscreen.ShaderGraph.FullscreenShaderGUI"
}