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

void ComputeLightingForAdditionalLights_float(float3 PositionWS, float3 NormalWS, float3 ViewDirectionWS, out float3 FinalColor)
{
    #if defined(SHADERGRAPH_PREVIEW)
    FinalColor = 0;
    #else
    int lightCount = GetAdditionalLightsCount();

    half3 lighting = 0;
    
    [unroll(8)]
    for (uint lightID = 0; lightID < lightCount; i++)
    {
        Light light = GetAdditionalLight(lightID, PositionWS);

        //Calculate Lighting

        //Diffuse
        half lambert = dot(NormalWS, light.direction);

        //Specular
        float3 h = normalize(light.direction + ViewDirectionWS);
        half NoH = dot(NormalWS, h);
        half blinnPhong = pow(saturate(NoH), 50.0f);

        half3 diffuseLighting = light.color * (lambert * light.shadowAttenuation * light.distanceAttenuation);
        half3 specularLighting = light.color * (blinnPhong * light.shadowAttenuation * light.shadowAttenuation);
        
        lighting += diffuseLighting + specularLighting;

    }
    FinalColor = lighting;
    #endif
}

#endif