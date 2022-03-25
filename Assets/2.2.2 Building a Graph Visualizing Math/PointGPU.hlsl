
#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
StructuredBuffer<float3> _Positions;
float _Step;
#endif
void ConfigureProcedural(){
    #if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
    float3 position = _Positions[unity_InstanceID];
    unity_ObjectToWorld = float4x4(
                          _Step, 0, 0, position.x,
                          0, _Step, 0, position.y,
                          0, 0, _Step, position.z,
                          0, 0, 0, 1);
    
    #endif
}

void ShaderGraphFunction_float(float3 In, out float3 Out){
    Out = In;
}
void ShaderGraphFunction_half(float3 In, out float3 Out){
    Out = In;
}