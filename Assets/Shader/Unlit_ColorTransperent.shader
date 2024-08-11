Shader "Unlit/ColorTransperent" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_Alpha ("Alpha", Float) = 1
		_Slide ("Slide", Range(0, 10)) = 0
		_Multiply ("Multiply", Range(0, 10)) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = _Color.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG
	}
	Fallback "Transparent/VertexLit"
}