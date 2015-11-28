float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 LightWorldViewProjection;
texture DiffuseTexture;
texture ShadowMap;
float3 LightDirection = float3(1, -1,  -1);
float4 AmbientLight = float4(0.5f, 0.5f, 0.5f, 1.0f);
float2 TexcoordOffset;
float2 TexcoordScale;
float ShadowScale = 1.0f / 1024.0f;

sampler2D DiffuseSampler = sampler_state {
	Texture = (DiffuseTexture);
	MagFilter = Point;
	MinFilter = Point;
	AddressU = Clamp;
	AddressV = Clamp;
};

sampler2D ShadowMapSampler = sampler_state {
	Texture = (ShadowMap);
	MagFilter = Point;
	MinFilter = Point;
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

struct VertexShaderShadowReceiverInput
{
	float4 Position : SV_Position0;
	float2 TexCoord : TEXCOORD0;
	float3 Normal   : NORMAL0;
};

struct VertexShaderShadowReceiverOutput
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
	float3 Normal   : TEXCOORD1;
	float4 LightPosition : TEXCOORD2;
};

float GetDiffuseFactor(float3 normal, float3 lightDir)
{
	return saturate(dot(normalize(normal), normalize(-lightDir)));
}

float4 CalculateLighting(float4 diffuseTextureColor, float diffuseFactor) 
{
	float alpha = diffuseTextureColor.a;
	float4 colorLit = diffuseTextureColor * (diffuseFactor + AmbientLight * (1-diffuseFactor));
	colorLit.a = alpha;
	return colorLit;
}

VertexShaderOutput DefaultVertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);
	output.Normal = mul(input.Normal, (float3x3)World);
	output.TexCoord = TexcoordOffset + float2(input.TexCoord.x * TexcoordScale.x, input.TexCoord.y * TexcoordScale.y);

	return output;
}

float4 DefaultPixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float diffuseFactor = GetDiffuseFactor(input.Normal, LightDirection);
	float4 colorFromTexture = tex2D(DiffuseSampler, input.TexCoord);

	return CalculateLighting(colorFromTexture, diffuseFactor);
}

VertexShaderShadowReceiverOutput DefaultVertexShaderShadowReceiverFunction(VertexShaderShadowReceiverInput input)
{
	VertexShaderShadowReceiverOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);
	output.Normal = mul(input.Normal, (float3x3)World);
	output.TexCoord = TexcoordOffset + float2(input.TexCoord.x * TexcoordScale.x, input.TexCoord.y * TexcoordScale.y);
	output.LightPosition = mul(worldPosition, LightWorldViewProjection);

	return output;
}

float4 DefaultPixelShaderShadowReceiverFunction(VertexShaderShadowReceiverOutput input) : COLOR0
{
	float2 projectedTexCoords;
	projectedTexCoords[0] = input.LightPosition.x / input.LightPosition.w / 2.0f + 0.5f;
	projectedTexCoords[1] = -input.LightPosition.y / input.LightPosition.w / 2.0f + 0.5f;

	float diffuseFactor = 0.0f;

	if ((saturate(projectedTexCoords).x == projectedTexCoords.x) && (saturate(projectedTexCoords).y == projectedTexCoords.y))
	{
		float depthStoredInShadowMap = tex2D(ShadowMapSampler, projectedTexCoords).r;
		float realDistance = input.LightPosition.z / input.LightPosition.w;
		if ((realDistance - 2.0f*ShadowScale) <= depthStoredInShadowMap)
		{
			diffuseFactor = GetDiffuseFactor(input.Normal, LightDirection);
		}
	}

	float4 colorFromTexture = tex2D(DiffuseSampler, input.TexCoord);

	return CalculateLighting(colorFromTexture, diffuseFactor);
}

technique Default
{
	pass Pass1
	{
		VertexShader = compile vs_4_0 DefaultVertexShaderFunction();
		PixelShader = compile ps_4_0 DefaultPixelShaderFunction();
	}
}

technique DefaultWithShadows
{
	pass Pass1
	{
		VertexShader = compile vs_4_0 DefaultVertexShaderShadowReceiverFunction();
		PixelShader = compile ps_4_0 DefaultPixelShaderShadowReceiverFunction();
	}
}

//----------------------------------------------------------------------------------------
//Shadow Caster Shader Code. Creates Shadow map.
//----------------------------------------------------------------------------------------

struct VSInputShadowCaster
{
	float4 Position : SV_POSITION;
};

struct VSOutputShadowReceiver
{
	float4 Position : POSITION;
	float4 DepthPosition : TEXCOORD0;
};

VSOutputShadowReceiver DepthVertexShader(VSInputShadowCaster input)
{
	VSOutputShadowReceiver output;

	input.Position.w = 1.0f;
	output.Position = mul(input.Position, LightWorldViewProjection);
	output.DepthPosition = output.Position;

	return output;
}

float4 DepthPixelShader(VSOutputShadowReceiver input) : SV_TARGET
{
	float depthValue = input.DepthPosition.z / input.DepthPosition.w;
	return float4(depthValue, depthValue, depthValue, 1.0f);
}

technique ShadowCaster
{
	pass Pass1
	{
		VertexShader = compile vs_4_0 DepthVertexShader();
		PixelShader = compile ps_4_0 DepthPixelShader();
	}
}

//-----------------------------------------------------------------------------
// Billboard
//-----------------------------------------------------------------------------

sampler2D BillboardSampler = sampler_state {
	Texture = (DiffuseTexture);
	MagFilter = Point;
	MinFilter = Point;
	AddressU = Clamp;
	AddressV = Clamp;
};

sampler2D LinearUnlitSampler = sampler_state {
	Texture = (DiffuseTexture);
	MagFilter = Linear;
	MinFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};

struct VertexShaderBillboardInput
{
	float4 Position : SV_Position0;
	float2 TexCoord : TEXCOORD0;
	float3 Normal   : NORMAL0;
};

struct VertexShaderBillboardOutput
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
	float3 Normal   : TEXCOORD1;
};

VertexShaderBillboardOutput VertexShaderBillboard(VertexShaderBillboardInput input)
{
	VertexShaderBillboardOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);
	output.Normal = mul(input.Normal, (float3x3)World);
	output.TexCoord = TexcoordOffset + float2(input.TexCoord.x * TexcoordScale.x, input.TexCoord.y * TexcoordScale.y);

	return output;
}

float4 PixelShaderFunctionBillboard(VertexShaderBillboardOutput input) : COLOR0
{
	float4 colorFromTexture = tex2D(BillboardSampler, input.TexCoord);
	float diffuseFactor = saturate(dot(normalize(input.Normal), normalize(-LightDirection)));

	return colorFromTexture;
}

float4 PixelShaderUnlitLinearSampled(VertexShaderBillboardOutput input) : COLOR0
{
	float4 colorFromTexture = tex2D(LinearUnlitSampler, input.TexCoord);
	float diffuseFactor = saturate(dot(normalize(input.Normal), normalize(-LightDirection)));

	return colorFromTexture;
}

technique DefaultBillboard
{
	pass Pass1
	{
		VertexShader = compile vs_4_0 VertexShaderBillboard();
		PixelShader = compile ps_4_0 PixelShaderFunctionBillboard();
	}
}

technique UnlitLinearSampled
{
	pass Pass1
	{
		VertexShader = compile vs_4_0 VertexShaderBillboard();
		PixelShader = compile ps_4_0 PixelShaderUnlitLinearSampled();
	}
}