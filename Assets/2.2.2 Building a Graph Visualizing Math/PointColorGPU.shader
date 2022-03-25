Shader "Graph/Point ColorGPU"{
    Properties{
        _Smoothess("Smoothess", Range(0,1))=0.5
    }

    SubShader{
        CGPROGRAM
        #include "PointGPU.hlsl"
        #pragma surface SurfaceConfigure Standard fullforwardshadows addshadow
        #pragma target 4.5
        #pragma instancing_options assumeuniformscaling procedural:ConfigureProcedural
        #pragma editor_sync_compilation

        struct Input{
            float3 worldPos;
        };

        float _Smoothess;
        // #if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
        // StructuredBuffer<float3> _Positions;
        // float _Step;
        // #endif

        // void ConfigureProcedural(){
        //     #if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
        //     float3 position = _Positions[unity_InstanceID];
        //     unity_ObjectToWorld = float4x4(
        //                           _Step, 0, 0, position.x,
        //                           0, _Step, 0, position.y,
        //                           0, 0, _Step, position.z,
        //                           0, 0, 0, 1);
            
        //     #endif
        // }

        void SurfaceConfigure(Input i, inout SurfaceOutputStandard surface){
            surface.Albedo.rg = i.worldPos.rg*0.5 +0.5;
            surface.Smoothness=_Smoothess;
        }

        ENDCG
    }
    Fallback "Diffuse"
}