// Upgrade NOTE: replaced '_CameraToWorld' with 'unity_CameraToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Error.mdl/Retro Sprites v2/Sprite Cutout Shadowcaster"
{
    Properties
	{
		[NoScaleOffset] _MainTex("Texture Array", 2DArray) = "white" {}
		[HDR] _Color("Color", color) = (1,1,1,1)
		[Toggle(_)] _light("Enable Lighting?", int) = 1
		[Toggle(_)] _A2C("Alpha To Coverage", int) = 0
		_alphaClip("Alpha Clipping Threshold", float) = 0.1
		_Dir("Number of Directions", int) = 8
		_frame("Manual Frame Number", float) = 0.0
		[Header(Columns (X)  Rows (Y)  Total Frames (Z)  Framerate (W))]
		_Params("Parameters", Vector) = (1,1,1,0)
		
    }
    SubShader
    {
        Tags { "Queue"="Alphatest" "RenderType"="Opaque" "DisableBatching"="True"}
		
        LOD 100
        
        Cull off
        Zwrite On
		AlphaToMask [_A2C]
        
        Pass {
			Tags {"LightMode" = "ForwardBase"}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma multi_compile _ VERTEXLIGHT_ON
            #include "UnityCG.cginc"
			#include "Lighting.cginc"
            #include "UnityLightingCommon.cginc"
			#include "cginc/sprite_lighting.cginc"
			#include "cginc/sprite_functions.cginc" 
			#include "cginc/sprite_opaque_vertfrag.cginc"
			ENDCG
			
        }



		Pass {
			Tags {"LightMode" = "Shadowcaster"}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "cginc/sprite_functions.cginc" 
			#include "cginc/sprite_shadowcaster.cginc"

			ENDCG
		}
    }
}