sampler baseTexture : register(s0);
float globalTime;
float R;
float G;
float B;

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = float4(0,0,0,1);
    color+=coords.y;
    color.rgb+=coords.x*float3(R,G,B);
    return color;
}
technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
