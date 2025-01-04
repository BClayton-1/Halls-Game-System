float3 simple_lighting_origin()
{
				float3 light;
				// Lighting
				
				// Interpolate the direction of maximum lighting from the lighting along each of
				// the axes in both the positive and negative directions
				
				// Measure the light hitting each normal direction
				/*
				float3 xp = max(0, ShadeSH9(float4(1,0,0,1)));
				float3 xn = max(0, ShadeSH9(float4(-1,0,0,1)));
				float3 yp = max(0, ShadeSH9(float4(0,1,0,1)));
				float3 yn = max(0, ShadeSH9(float4(0,-1,0,1)));
				float3 zp = max(0, ShadeSH9(float4(0,0,1,1)));
				float3 zn = max(0, ShadeSH9(float4(0,0,-1,1)));
				
				// Assign a weight to each direction based on the greater
				// of the intensities of the light falling on the positive
				// and negative directions
				float x_weight = max(length(xp), length(xn));
				x_weight += -2.0*x_weight*step(xp,xn); 
				float y_weight = max(length(yp), length(yn));
				y_weight += -2.0*y_weight*step(yp,yn);
				float z_weight = max(length(zp), length(zn));
				z_weight += -2.0*z_weight*step(zp,zn);
				
				float4 max_dir = x_weight*float4(1,0,0,0) + y_weight*float4(0,1,0,0) + z_weight*float4(0,0,1,0);
				max_dir = normalize(max_dir);
				*/
				float3 MaxBaked = float3(length(unity_SHAr), length(unity_SHAg), length(unity_SHAb));
				half nl = length(_WorldSpaceLightPos0.xyz);
				light = nl * _LightColor0 + MaxBaked;
				
				float3 vertexLighting = float3(0.0, 0.0, 0.0);
				#ifdef VERTEXLIGHT_ON
				float4 worldOrigin = mul(unity_ObjectToWorld, float4(0,0,0,1));
				for (int index = 0; index < 4; index++)
				{    
					float4 lightPosition = float4(unity_4LightPosX0[index], 
					unity_4LightPosY0[index], 
					unity_4LightPosZ0[index], 1.0);
 
					float3 vertexToLightSource = 
					lightPosition.xyz - worldOrigin.xyz;        
					//float3 lightDirection = normalize(vertexToLightSource);
					float squaredDistance = 
					dot(vertexToLightSource, vertexToLightSource);
					float attenuation = 1.0 / (1.0 + 
					unity_4LightAtten0[index] * squaredDistance);
					float3 diffuseReflection = attenuation 
					* unity_LightColor[index].rgb;         
 
					vertexLighting = vertexLighting + diffuseReflection;
				}
				#endif
				
				light += vertexLighting;
				
				return light;
}


float3 simple_lighting(float3 pos)
{
	float3 light;
	float3 MaxBaked = float3(length(unity_SHAr), length(unity_SHAg), length(unity_SHAb));
	half nl = length(_WorldSpaceLightPos0.xyz);
	light = nl * _LightColor0 + MaxBaked;

	float3 vertexLighting = float3(0.0, 0.0, 0.0);
#ifdef VERTEXLIGHT_ON
	for (int index = 0; index < 4; index++)
	{
		float4 lightPosition = float4(unity_4LightPosX0[index],
			unity_4LightPosY0[index],
			unity_4LightPosZ0[index], 1.0);

		float3 vertexToLightSource =
			lightPosition.xyz - pos;
		//float3 lightDirection = normalize(vertexToLightSource);
		float squaredDistance =
			dot(vertexToLightSource, vertexToLightSource);
		float attenuation = 1.0 / (1.0 +
			unity_4LightAtten0[index] * squaredDistance);
		float3 diffuseReflection = attenuation
			* unity_LightColor[index].rgb;

		vertexLighting = vertexLighting + diffuseReflection;
	}
#endif

	light += vertexLighting;

	return light;
}