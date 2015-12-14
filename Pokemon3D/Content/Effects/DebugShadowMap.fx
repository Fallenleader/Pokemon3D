texture DepthMap;
sampler2D DepthMapSampler = sampler_state {
	Texture = (DepthMap);
	MagFilter = Point;
	MinFilter = Point;
	AddressU = Clamp;
	AddressV = Clamp;
};

float4 DebugShadowMapPixelShader(float4 position : SV_Position, float4 color : COLOR0, float2 tex_coord : TEXCOORD0) : SV_Target0
{
	float4 depth = tex2D(DepthMapSampler, tex_coord);
	return float4(depth.r, depth.r, depth.r, 1.0f);
}
technique Default
{
	pass Pass1
	{
		PixelShader = compile ps_4_0 DebugShadowMapPixelShader();
	}
}