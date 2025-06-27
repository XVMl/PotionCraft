sampler baseTexture : register(s0);

float globalTime;
float time;
 
float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float2 pos = 0.5 - coords;
    
    float dis = 0.1*0.3 / length(pos);
    float pow_dis = pow(abs(dis), 1.6);
    float3 color = pow_dis * float3(1, 0.5, 0.25);
    return float4(color, 1);
}

technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}