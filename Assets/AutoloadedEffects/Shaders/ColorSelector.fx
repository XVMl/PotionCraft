sampler baseTexture : register(s0);

float globalTime;
float R;
float G;
float B;
float4 PixelShaderFunction(float4 samplercolor : TEXCOORD0, float2 coords : TEXCOORD0) : COLOR0
{
    float3 icolor = float3(R, G, B);
    float4 color = coords.y - coords.x * (float3(1,1,1) - icolor);
    return color;
}

technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}