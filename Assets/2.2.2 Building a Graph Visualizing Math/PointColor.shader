Shader "Graph/Point Color"{
    Properties{
        _Smoothess("Smoothess", Range(0,1))=0.5
    }

    SubShader{
        CGPROGRAM
        #pragma surface SurfaceConfigure Standard fullforwardshadows addshadow
        #pragma target 3.0

        struct Input{
            float3 worldPos;
        };

        float _Smoothess;

        void SurfaceConfigure(Input i, inout SurfaceOutputStandard surface){
            surface.Albedo.rg = i.worldPos.rg*0.5 +0.5;
            surface.Smoothness=_Smoothess;
        }

        ENDCG
    }
    Fallback "Diffuse"
}