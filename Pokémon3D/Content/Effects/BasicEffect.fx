float4x4 World;
float4x4 View;
float4x4 Projection;
texture DiffuseTexture;
float3 LightDirection = float3(1, -1, 1);

sampler DiffuseSampler = sampler_state
{
	MINFILTER = LINEAR;
	MAGFILTER = LINEAR;
	MIPFILTER = LINEAR;
	Texture = <DiffuseTexture>;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
	float3 Normal   : NORMAL0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
	float3 Normal   : TEXCOORD1;
};

VertexShaderOutput DefaultVertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);
	output.Normal = mul(float4(input.Normal, 0.0f), World).xyz;
	output.TexCoord = input.TexCoord;

	return output;
}

float4 DefaultPixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 colorFromTexture = tex2D(DiffuseSampler, input.TexCoord);
	float diffuseFactor = saturate(dot(normalize(input.Normal), normalize(-LightDirection)));

	return colorFromTexture * diffuseFactor;
}

technique Default
{
	pass Pass1
	{
		VertexShader = compile vs_4_0 DefaultVertexShaderFunction();
		PixelShader = compile ps_4_0 DefaultPixelShaderFunction();
	}
}