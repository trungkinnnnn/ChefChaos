Shader "FlatKit/Water" {
	Properties {
		[KeywordEnum(Linear, Gradient Texture)] _ColorMode ("[FOLDOUT(Colors){9}]Source{Colors}", Float) = 0
		_ColorShallow ("[_COLORMODE_LINEAR]Shallow", Vector) = (0.35,0.6,0.75,0.8)
		_ColorDeep ("[_COLORMODE_LINEAR]Deep", Vector) = (0.65,0.9,1,1)
		[NoScaleOffset] _ColorGradient ("[_COLORMODE_GRADIENT_TEXTURE]Gradient", 2D) = "white" {}
		_FadeDistance ("Shallow Depth", Float) = 0.5
		_WaterDepth ("Gradient Size", Float) = 5
		_LightContribution ("Light Color Contribution", Range(0, 1)) = 0
		_WaterClearness ("Transparency", Range(0, 1)) = 0.3
		_ShadowStrength ("Shadow Strength", Range(0, 1)) = 0.35
		_CrestColor ("[FOLDOUT(Crest){3}]Color{Crest}", Vector) = (1,1,1,0.9)
		_CrestSize ("Size{Crest}", Range(0, 1)) = 0.1
		_CrestSharpness ("Sharp transition{Crest}", Range(0, 1)) = 0.1
		[KeywordEnum(None, Round, Grid, Pointy)] _WaveMode ("[FOLDOUT(Wave Geometry){6}]Shape{Wave}", Float) = 1
		_WaveSpeed ("[!_WAVEMODE_NONE]Speed{Wave}", Float) = 0.5
		_WaveAmplitude ("[!_WAVEMODE_NONE]Amplitude{Wave}", Float) = 0.25
		_WaveFrequency ("[!_WAVEMODE_NONE]Frequency{Wave}", Float) = 1
		_WaveDirection ("[!_WAVEMODE_NONE]Direction{Wave}", Range(-1, 1)) = 0
		_WaveNoise ("[!_WAVEMODE_NONE]Noise{Wave}", Range(0, 1)) = 0.25
		[KeywordEnum(None, Gradient Noise, Texture)] _FoamMode ("[FOLDOUT(Foam){12}]Source{Foam}", Float) = 1
		[NoScaleOffset] _NoiseMap ("[_FOAMMODE_TEXTURE]Texture{Foam}", 2D) = "white" {}
		_FoamColor ("[!_FOAMMODE_NONE]Color{Foam}", Vector) = (1,1,1,1)
		[Space] _FoamDepth ("[!_FOAMMODE_NONE]Shore Depth{Foam}", Float) = 0.5
		_FoamNoiseAmount ("[!_FOAMMODE_NONE]Shore Blending{Foam}", Range(0, 1)) = 1
		[Space] _FoamAmount ("[!_FOAMMODE_NONE]Amount{Foam}", Range(0, 3)) = 0.25
		[Space] _FoamScale ("[!_FOAMMODE_NONE]Scale{Foam}", Range(0, 3)) = 1
		_FoamStretchX ("[!_FOAMMODE_NONE]Stretch X{Foam}", Range(0, 10)) = 1
		_FoamStretchY ("[!_FOAMMODE_NONE]Stretch Y{Foam}", Range(0, 10)) = 1
		[Space] _FoamSharpness ("[!_FOAMMODE_NONE]Sharpness{Foam}", Range(0, 1)) = 0.5
		[Space] _FoamSpeed ("[!_FOAMMODE_NONE]Speed{Foam}", Float) = 0.1
		_FoamDirection ("[!_FOAMMODE_NONE]Direction{Foam}", Range(-1, 1)) = 0
		_RefractionFrequency ("[FOLDOUT(Refraction){4}]Frequency", Float) = 35
		_RefractionAmplitude ("Amplitude", Range(0, 0.1)) = 0.01
		_RefractionSpeed ("Speed", Float) = 0.1
		_RefractionScale ("Scale", Float) = 1
		[ToggleOff] [HideInInspector] _Opaque ("Opaque", Float) = 0
		[HideInInspector] _QueueOffset ("Queue offset", Float) = 0
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
	//CustomEditor "FlatKitWaterEditor"
}