sampler baseTexture : register(s0);

float globalTime;

float3 pal(float t ,float3 a,float3 b,float3 c,float3 d)
{
    return a + b * cos(6.28318 * (c * t + d));
}

float4 PixelShaderFunction(float4 samplercolor : TEXCOORD0, float2 coords : TEXCOORD0) : COLOR0
{
    float3 color = pal(-coords.x, float3(0.5, 0.5, 0.5), float3(0.5, 0.5, 0.5), float3(1.0, 1.0, 1.0), float3(0,0.33,0.67));
    return float4(color, 1);
} 

technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}