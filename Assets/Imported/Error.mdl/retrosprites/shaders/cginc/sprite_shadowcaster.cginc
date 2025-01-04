struct VertIn
{
float4 pos : POSITION;
float2 uv : TEXCOORD0;
};

struct v2f
{
	float3 uv : TEXCOORD0;
	float4 pos : SV_POSITION;
};

UNITY_DECLARE_TEX2DARRAY(_MainTex);
float4 _Params;
half _alphaClip;
uint _light;
half _frame;
int _Dir;
float4 _Color;

v2f vert(VertIn v)
{

	v2f o;

	float4 cameraPos = obj_cam_pos();
	//float4 cameraPos = mul(unity_WorldToObject, _WorldSpaceLightPos0);

	float4 pos = rotate_sprite(cameraPos, v.pos);
	o.pos = UnityObjectToClipPos(pos);

	int dir = sprite_dir(_Dir, cameraPos);
	o.uv.z = dir;
	o.uv.xy = sprite_sheet_uvs(v.uv, _Params, _frame);

	return o;
}



fixed4 frag(v2f i) : SV_Target
{
	float4 finalColor = UNITY_SAMPLE_TEX2DARRAY(_MainTex, i.uv) * _Color;

	clip(finalColor.a - _alphaClip);

	return finalColor;
}