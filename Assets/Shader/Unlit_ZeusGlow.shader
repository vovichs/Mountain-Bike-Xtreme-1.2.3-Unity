Shader "Unlit/ZeusGlow" {
	Properties {
		_MoonTex ("Moon Texture", 2D) = "black" {}
		_ColorA ("Color A", Vector) = (1,1,1,1)
		_ColorB ("Color B", Vector) = (0,0,0,1)
		_Slide ("Slide", Range(0, 2)) = 0.5
		_Multiply ("Multiply", Range(0, 20)) = 6
		_Alpha ("Alpha", Float) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = 1;
		}
		ENDCG
	}
}