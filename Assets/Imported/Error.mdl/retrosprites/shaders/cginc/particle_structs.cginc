struct VertIn
{
	float4 pos : POSITION;
	float4 color : COLOR;
	float4 uv_center : TEXCOORD0;
	float4 center_velocity: TEXCOORD1;
};

struct v2f
{
	float3 uv : TEXCOORD0;
	float4 pos : SV_POSITION;
	float4 color : COLOR;
	float3 lighting : TEXCOORD1;
};