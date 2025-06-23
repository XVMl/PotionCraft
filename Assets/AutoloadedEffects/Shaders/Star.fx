sampler baseTexture : register(s0);
//sampler noiseTexture : register(s1);

float globalTime;
float time;
float Difference(float x,float y)
{
    return abs(x - y);
}
float ColorDodge(float x, float y)
{
    return float(saturate((y / max(1 - x, 0.00001))));
}
float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 MainColor = float4(0.4, 0.1, 0.7, 1);
    float4 EdgeColor = float4(1, 0.894, 0.02, 1);
    float x_diff = Difference(coords.x, coords.y);
    float y_diff = Difference(coords.x, 1-coords.y);
    float x_cd = ColorDodge(x_diff, y_diff);
    float y_cd = ColorDodge(y_diff, x_diff); 
    //这部分星星边缘将变为直线
    
    //float l_Main = step(x_cd, 0.4)+ step(y_cd, 0.4);
    //float l_Edge = l_Main - 2*(step(x_cd, 0.36) + step(y_cd, 0.36));
    
    //这部分星星边缘将变为曲线
    float pow_color = pow(x_cd * y_cd, 1.2); 
    float Main = step(pow_color, 0.3);
    float edge = step(x_cd * y_cd, 0.2);
    float Edge = Main - edge;
    MainColor *= Main;
    EdgeColor *= Edge;
    float4 color = float4(0, 0, 0, 1) + MainColor + EdgeColor;
    if (!any(color.r))
    {
        return float4(0, 0, 0, 0);
    }
    return color;
}

technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}