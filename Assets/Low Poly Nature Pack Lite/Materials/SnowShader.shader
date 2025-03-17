Shader "Custom/SnowShader"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _SnowColor ("Snow Color", Color) = (1, 1, 1, 1)
        _SnowStrength ("Snow Strength", Range(0, 1)) = 0.5
        _SnowThreshold ("Snow Threshold", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        sampler2D _MainTex;
        float4 _SnowColor;
        float _SnowStrength;
        float _SnowThreshold;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldNormal;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Textura base
            fixed4 baseColor = tex2D(_MainTex, IN.uv_MainTex);

            // Factor de nieve basado en la normal del mundo
            float snowFactor = saturate((IN.worldNormal.y - _SnowThreshold) / (1 - _SnowThreshold));

            // Mezcla entre la textura base y el color de nieve
            o.Albedo = lerp(baseColor.rgb, _SnowColor.rgb, snowFactor * _SnowStrength);
            o.Metallic = 0;
            o.Smoothness = 0.5;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
