#ifndef LIGHTING_CUSTOM_INCLUDED
#define LIGHTING_CUSTOM_INCLUDED

//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

void GetMainLight_float(float3 PositionWS, out half3 Color, out float3 Direction, out float ShadowAttenuation)
{
    #if defined (SHADERGRAPH_PREVIEW)
    Color = 1;
    Direction = normalize(float3(1, 1, -1));
    ShadowAttenuation = 1;
    #else
    float4 shadowCord = TransformWorldToShadowCoord(PositionWS);
    Light light = GetMainLight();
    Color = light.color;
    Direction = light.direction;
    ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
    float shadowIntensity = GetMainLightShadowStrength();

    ShadowAttenuation = SampleShadowmap(shadowCord, TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture), shadowSamplingData, shadowIntensity, false);
    #endif
    
}

#endif