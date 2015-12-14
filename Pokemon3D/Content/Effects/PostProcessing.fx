texture SourceMap;
float2 InvScreenSize;

sampler2D SourceMapSampler = sampler_state {
	Texture = (SourceMap);
	MagFilter = Point;
	MinFilter = Point;
	AddressU = Clamp;
	AddressV = Clamp;
};

float4 GaussianBlurPS(float4 position : SV_Position, float4 color : COLOR0, float2 tex_coord : TEXCOORD0) : SV_Target0
{
	return (tex2D(SourceMapSampler, tex_coord) 
		+ tex2D(SourceMapSampler, tex_coord + InvScreenSize * 1.0f)
		+ tex2D(SourceMapSampler, tex_coord + InvScreenSize * 2.0f)
		+ tex2D(SourceMapSampler, tex_coord + InvScreenSize * -1.0f)
		+ tex2D(SourceMapSampler, tex_coord + InvScreenSize * -2.0f)) / 5.0f;
}
technique GaussianBlur
{
	pass Pass1
	{
		PixelShader = compile ps_4_0 GaussianBlurPS();
	}
}