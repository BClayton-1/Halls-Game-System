﻿// Upgrade NOTE: replaced '_CameraToWorld' with 'unity_CameraToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Error.mdl/Retro Sprites v2/Particle Cutout"
{
    Properties
	{
		[NoScaleOffset] _MainTex("Texture Array", 2DArray) = "white" {}
		[HDR] _Color("Color", color) = (1,1,1,1)
		[Toggle(_)] _light("Enable Lighting?", int) = 1
		[Toggle(_)] _A2C("Alpha To Coverage", int) = 0
		_alphaClip("Alpha Clipping Threshold", float) = 0.1
		_Dir("Number of Directions", int) = 8
    }
    SubShader
    {
        Tags { "Queue"="Alphatest" "RenderType"="Opaque"}
		
        LOD 100
        
        Cull off
        Zwrite On
		AlphaToMask [_A2C]
        
        Pass {
			Tags {"LightMode" = "ForwardBase"}
			Stencil {
					Ref 13
					Comp NotEqual
					Pass Keep
			}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma multi_compile _ VERTEXLIGHT_ON
            #include "UnityCG.cginc"
			#include "Lighting.cginc"
            #include "UnityLightingCommon.cginc"
			#include "cginc/sprite_lighting.cginc"
			#include "cginc/sprite_functions.cginc" 
			#include "cginc/particle_structs.cginc"
           
			UNITY_DECLARE_TEX2DARRAY(_MainTex);
			float4 _Params;
			half _alphaClip;
			uint _light;
			half _frame;
			int _Dir;
			float4 _Color;
			
			#include "cginc/particle_vert.cginc"
			#include "cginc/particle_frag_cutout.cginc"
            ENDCG
        }
    }
}