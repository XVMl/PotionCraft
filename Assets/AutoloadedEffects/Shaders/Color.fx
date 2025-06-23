sampler baseTexture : register(s0);

float globalTime;
float4 PixelShaderFunction(float4 samplercolor :TEXCOORD0,float2 coords : TEXCOORD0) : COLOR0
{
    //if(!any(samplercolor)) 
    //{
    //    samplercolor = float4(1, 1, 1, 1);
    //}
    float4 color = tex2D(baseTexture, coords);
    return samplercolor;
}



technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}