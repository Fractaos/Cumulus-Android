#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif
#define D3DXSHADER_ENABLE_BACKWARDS_COMPATIBILITY

sampler s0;
static const float PI = 3.14159265f;
float percent;
float R, G, B, A;
float4 colorCircle;

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 color = tex2D(s0, input.TextureCoordinates);
	float2 coords = input.TextureCoordinates;
	colorCircle = float4(R, G, B, A);
	float dx = coords.x - 0.5f;
	float dy = coords.y - 0.5f;
	float distance = sqrt(pow(0 - dx, 2) + pow(0 - dy, 2));
	if(distance <= 0.5)
	{
		float radian = atan2(0 - dy, 0 - dx);
		float angle = radian * (180 / PI) - 90;
		if(angle<0)
			angle += 360;

		if (angle <= 360 * percent && (percent >= 0 && percent <= 1))
		{
			color = float4(colorCircle.r, colorCircle.g, colorCircle.b, colorCircle.a);
		}
		else
			color = float4(0.0f, 0.0f, 0.0f, 0.0f);

	}
	else
		color = float4(0.0f, 0.0f, 0.0f, 0.0f);

	return color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};