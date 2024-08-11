Shader "Unlit/Cloud" {
	Properties {
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_TintColor ("TintColor", Vector) = (1,1,1,1)
		_ColorA ("ColorA", Vector) = (1,1,1,1)
		_ColorB ("ColorB", Vector) = (1,1,1,1)
		_ColorC ("ColorC", Vector) = (1,1,1,1)
		_AddColor ("AddColor", Range(0, 100)) = 0
		_Middle ("Middle", Range(0.001, 0.999)) = 0.5
		_Alpha ("Alpha", Range(0, 1)) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
}