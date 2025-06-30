sampler baseTexture : register(s0);

float globalTime;
float time;
 
float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float2 pos = 0.5 - coords;
    
    float dis = 0.1*0.3 / length(pos)*time;
    float pow_dis = pow(abs(dis), 1.6);
    float4 color = pow_dis * float4(1, 0.5, 0.25,1);
    return color;
}

technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}