float4x4 World;
float4x4 View;
float4x4 Projection;
texture DiffuseTexture;
float3 LightDirection = float3(1, -1,  -1);

sampler2D DiffuseSampler = sampler_state {
	Texture = (DiffuseTexture);
	MagFilter = Linear;
	MinFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};

struct VertexShaderInput
{
	float4 Position : SV_Position0;
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
	output.Normal = mul(input.Normal, (float3x3)World);
	output.TexCoord = input.TexCoord;

	return output;
}

float4 DefaultPixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 colorFromTexture = tex2D(DiffuseSampler, input.TexCoord);
	float diffuseFactor = saturate(dot(normalize(input.Normal), normalize(-LightDirection)));

	return colorFromTexture *diffuseFactor;
}

technique Default
{
	pass Pass1
	{
		VertexShader = compile vs_4_0 DefaultVertexShaderFunction();
		PixelShader = compile ps_4_0 DefaultPixelShaderFunction();
	}
}