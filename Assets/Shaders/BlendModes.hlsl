#ifndef _BLENDMODES_
#define _BLENDMODES_

float3 blendOverlay(float3 base, float3 blend)
{
    return lerp(
        sqrt(base) * (2.0 * blend - 1.0) + 2.0 * base * (1.0 - blend),
        2.0 * base * blend + base * base * (1.0 - 2.0 * blend),
        step(base, float3(0.5, 0.5, 0.5))
    );
}

float3 blendScreen(float3 base, float3 blend){
    return 1.0 - (1.0 - base) * (1.0 - blend);
}

float3 blendColorBurn(float3 base, float3 blend){
    return 1.0 - (1.0 - base) / blend;
}

float3 blendColorDodge(float3 base, float3 blend){
    return base / (1.0 - blend);
}

#endif