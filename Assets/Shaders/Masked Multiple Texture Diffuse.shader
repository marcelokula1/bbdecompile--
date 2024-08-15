Shader "Custom/Masked and Layered Diffuse" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _SecondTex ("Secondary (RGB) Trans (A)", 2D) = "black" {}
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _Mask ("Mask (RGB) Trans (A)", 2D) = "white" {}
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }

    SubShader {
        Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
        LOD 200
        
        CGPROGRAM
            #pragma surface surf Lambert alphatest:_Cutoff

            sampler2D _MainTex;
            sampler2D _SecondTex;
            sampler2D _Mask;
            fixed4 _Color;

            struct Input {
                float2 uv_MainTex;
            };

            void surf (Input IN, inout SurfaceOutput o) {
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
                fixed4 m = tex2D(_Mask, IN.uv_MainTex);
                fixed4 s = tex2D(_SecondTex, IN.uv_MainTex) * _Color;

                o.Albedo = lerp(c.rgb, s.rgb, s.a);

                o.Alpha = c.a * m.a * m.rgb + s.a * (1 - m.a); 
            }
        ENDCG
    }

    Fallback "Legacy Shaders/Transparent/Cutout/VertexLit"
}
