//https://www.shadertoy.com/view/3ftSz2
sampler baseTexture : register(s0);
sampler noiseTexture : register(s1);

float blurOffset;
float4 colorMask; 
bool invert;
float globalTime;
float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 laserColor = float4(0.3, 0.45, 1, 1);
    float4 white = float4(1.0, 1.0, 1.0, 1.0);
    float intensity = 1.0 - abs(coords.y - 0.5);
    intensity = pow(intensity, 6);
    
    float2 noise = float2(coords.x * 0.2 - globalTime, coords.y * 1);
    float noiseIntensity = tex2D(noiseTexture, noise).r;
    
    float4 noiseColor;
    noiseColor.r = noiseIntensity * laserColor.r;
    noiseColor.g = noiseIntensity * laserColor.g;
    noiseColor.b = noiseIntensity * laserColor.b;
    noiseColor.a = 0.6;
    float4 effectColor = noiseColor * intensity * 3;
    
    float centerIntensity = 20.0;
    //Mix it with the color white raised to a higher exponent to make the center white beam
    effectColor = effectColor + white * centerIntensity * (pow(intensity, 6.0) * noiseIntensity);
    
    return effectColor;
}

technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}