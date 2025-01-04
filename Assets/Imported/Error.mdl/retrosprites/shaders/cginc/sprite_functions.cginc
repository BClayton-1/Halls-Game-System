/*
 * Returns the world space postion of the camera, corrected to be the midpoint between the eyes for VR users 
 */
float4 wrld_cam_pos()
{
	#if UNITY_SINGLE_PASS_STEREO
		float4 cameraPos = float4((unity_StereoWorldSpaceCameraPos[0] + unity_StereoWorldSpaceCameraPos[1])*0.5, 1);
	#else
		float4 cameraPos = mul(unity_CameraToWorld, float4(0,0,0,1));
	#endif
	return cameraPos;
}


/*
 * Returns the object space position of the camera
 */
float4 obj_cam_pos()
{
	float4 cameraPos = wrld_cam_pos();
	cameraPos =  mul(unity_WorldToObject, cameraPos);
	return cameraPos;
}


/*
 * Given the position of a vertex and camera, rotates the vertex around the coordinate space center in the xz plane so that the 
 * Z-axis points towards the camera.
 */
float4 rotate_sprite(float4 cameraPos, float4 vertPos)
{
	float len = distance(float2(0, 0), float2(cameraPos[0], cameraPos[2]));
	
	float cosa = (cameraPos[2])/len;
	float sina = (cameraPos[0])/len;
	
	float4x4 R = float4x4(
		cosa,	0,	sina,	0,
		0,		1,	0,		0,
		-sina,	0,	cosa,	0,
		0,		0,	0,		1);
	
	vertPos = mul(R, vertPos);
	return vertPos;
}

float4 rotate_camera(float4 cameraPos, float3 forward)
{
	float cosa = forward.z;
	float sina = -forward.x;

	float4x4 R = float4x4(
		cosa, 0, sina, 0,
		0, 1, 0, 0,
		-sina, 0, cosa, 0,
		0, 0, 0, 1);

	cameraPos = mul(R, cameraPos);
	return cameraPos;
}


/*
 * Calculates the direction on the sprite sheet to use. starts at 0 for +z and increases clockwise 
 */
int sprite_dir(int totalDivisions, float4 cameraPos)
{
	//Get the angle between the camera and (0,0,-1) in the xz plane, ranges from -pi to pi
	float angle = atan2(-cameraPos[0], -cameraPos[2]);
	//Calculate the fraction of 2pi each direction occupies (eg for 8 directions, each division is 0.25*pi)
	float div = 1.0 / ((float)totalDivisions);
	// Calculate which texture in the texture array to use. Starts at 0 for the +z spritesheet and increases
	// counter-clockwise. 
	int dir = floor((angle + (1 + div)*UNITY_PI)/(UNITY_TWO_PI*div)) % totalDivisions;
	return dir;
}


/*
 * Transforms the given UVs (assuming a 0-1 range on both axes) to only span the portion of the
 * active tile in the sprite sheet. Params contains in order the number of columns, the number of rows,
 * the total number of tiles, and the framerate.
 */
float2 sprite_sheet_uvs(float2 uv, float4 Params, float manualFrame)
{
	//From the frame number, get the row and column of the frame on the sprite sheet.
	uint3 dim = floor(Params.xyz);
	uint frame_num = floor(fmod(_Time[1]*Params.w + manualFrame, Params.z));
	int2 frame = int2(frame_num % dim.x, frame_num / dim.x);

	float2 tile_uv = float2((uv.x + frame[0])/Params.x, ((uv.y - frame[1])/Params.y) + (Params.y - 1.0)/Params.y);
	return tile_uv;
}