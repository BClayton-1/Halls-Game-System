struct VertIn
{
	float4 pos : POSITION;
	float2 uv : TEXCOORD0;
};

struct v2f
{
	float3 uv : TEXCOORD0;
	float4 pos : SV_POSITION;
	float3 color : COLOR;
	UNITY_FOG_COORDS(1)
};