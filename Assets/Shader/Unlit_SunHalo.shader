Shader "Unlit/SunHalo" {
	Properties {
		_ColorA ("Color A", Vector) = (1,1,1,1)
		_ColorB ("Color B", Vector) = (0,0,0,1)
		_Slide ("Slide", Range(0, 1)) = 0.5
		_HoleMap ("HoleMap", 2D) = "white" {}
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