Shader "Hidden/FlatKit/OutlineWrap" {
	Properties {
		_EdgeColor ("Outline Color", Vector) = (0.3137255,0.6509804,0.2784314,1)
		_Thickness ("Thickness", Range(0, 5)) = 0
		_ColorThresholdMin ("Min Color Threshold", Range(0, 1)) = 0
		_ColorThresholdMax ("Max Color Threshold", Range(0, 1)) = 0.25
		_DepthThresholdMin ("Min Depth Threshold", Range(0, 1)) = 0
		_DepthThresholdMax ("Max Depth Threshold", Range(0, 1)) = 0.25
		_NormalThresholdMin ("Min Normals Threshold", Range(0, 1)) = 0.5
		_NormalThresholdMax ("Max Normals Threshold", Range(0, 1)) = 1
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