v2f vert(VertIn v)
{

	v2f o;

	// Get the position in between the two cameras if the viewer is in VR, otherwise get the position of the
	// camera. If you don't do this, the sprite will look very stereo-incorrect as it will be oriented toward
	// both eyes simultaneously
	float4 cameraPos = obj_cam_pos();

	float4 pos = rotate_sprite(cameraPos, v.pos);
	o.pos = UnityObjectToClipPos(pos);
	UNITY_TRANSFER_FOG(o, o.pos);
	int dir = sprite_dir(_Dir, cameraPos);
	o.uv.z = dir;
	o.uv.xy = sprite_sheet_uvs(v.uv, _Params, _frame);
	o.color = simple_lighting_origin();
	o.color = _light == 1 ? o.color : float3(1, 1, 1);

	return o;
}