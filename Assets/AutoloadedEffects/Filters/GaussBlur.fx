sampler baseTexture : register(s0);

float2 screenscalerevise;
float gauss[3][3] =
{
	0.075, 0.124, 0.075,  
    0.124, 0.204, 0.124,
    0.075, 0.124, 0.075 
};
float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(baseTexture, coords);
	if (!any(color))
		return color;
	float dx = 2 / screenscalerevise.x;
	float dy = 2 / screenscalerevise.y;
	float4 result = float4(0, 0, 0, 0);
	result += gauss[0][0] * tex2D(baseTexture, float2(coords.x - dx, coords.y - dy));
	result += gauss[0][1] * tex2D(baseTexture, float2(coords.x, coords.y - dy));
	result += gauss[0][2] * tex2D(baseTexture, float2(coords.x + dx, coords.y - dy));
	result += gauss[1][0] * tex2D(baseTexture, float2(coords.x - dx, coords.y));
	result += gauss[1][1] * tex2D(baseTexture, float2(coords.x, coords.y));
	result += gauss[1][2] * tex2D(baseTexture, float2(coords.x + dx, coords.y));
	result += gauss[2][0] * tex2D(baseTexture, float2(coords.x - dx, coords.y + dy));
	result += gauss[2][1] * tex2D(baseTexture, float2(coords.x, coords.y + dy));
	result += gauss[2][2] * tex2D(baseTexture, float2(coords.x + dx, coords.y + dy));

	//for (int i = -1; i <= 1; i++)
	//{
	//	for (int j = -1; j <= 1; j++)
	//	{
	//		color += gauss[i + 1][j + 1] * tex2D(baseTexture, float2(coords.x + dx * i, coords.y + dy * j));
	//	}
	//}
	return result;
    
}
technique Technique1
{
	pass AutoloadPass
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}