//https://www.shadertoy.com/view/3ftSz2
sampler baseTexture : register(s0);

float blurOffset;
float4 colorMask;
bool invert;
float globalTime;

float2 screenscalerevise;
float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float2 r = screenscalerevise;
    // Variables for ray marching: i=ray steps, j=plane index, d=closest distance, 
    // D=temp distance, z=ray depth, T=animated time
    float i, j, d, D, z, T = .3 * globalTime;
  
    // o=accumulated color, p=ray position, P=transformed position, U=utility vector
    float4 o, p, P, U = float4(0, 1, 2, 3);
  
    for (;i++<77;z+=1E-3)
    {
        for (p = z * normalize(float3(coords - .5 * r, r.y)).xyzz, p.z -= d = j = 4.; ++j < 9; d = min(d, D))
        {
            D = 2. / (j - 3.) + p.y / 3.;
            P = p;
            P.xz *= float2x2(cos(j - T + p.y + 11. * U.xywx));
            P = abs(P);
            D = min(max(P.z, P.x - D) + 2E-2, length(P.xz - D * U.yx));
            P = 1.2 + sin(j + U.xyzy + z);
            o += P.w * P / D;
        }
    }
    float4 Color = tanh(o / 1E3);
    return Color;
}

technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}