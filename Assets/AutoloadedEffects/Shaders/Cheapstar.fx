sampler baseTexture : register(s0);
//sampler noiseTexture : register(s1);

float globalTime;
float time;
float Cheapstar(float2 uv,float anim)
{
    uv = abs(uv);
    float2 pos = min(uv.xy / uv.yx, anim);
    float p = (2 - pos.x - pos.y);
    return (2 + p * (p * p - 1.5)) / (uv.x + uv.y);
}

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float time = 1.0;
    coords *= 2 * (cos(time*2.0) - 2.5);
    float anim = sin(time * 12) * 0.1 + 1;
    float4 color = float4(Cheapstar(coords, anim) * float3(0.3,0.1,0.5), 1);
    return color;
}

technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}