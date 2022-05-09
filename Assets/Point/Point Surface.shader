Shader "Graph/Point Surface"
{
    Properties
    {
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
    }

    SubShader
    {
        CGPROGRAM

        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface configSurf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        struct Input
        {
            float3 worldPos;
        };

        float _Smoothness;

        void configSurf(Input input, inout SurfaceOutputStandard surface)
        {
            surface.Smoothness = _Smoothness;
            surface.Albedo.rg = input.worldPos.xy * 0.5 + 0.5;
        }

        ENDCG
    }

    FallBack "Diffuse"
}
