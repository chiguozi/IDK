// Unlit shader. Simplest possible textured shader.
// - SUPPORTS lightmap
// - no lighting
// - no per-material color

Shader "Mobile/UnlitCullOff (Supports Lightmap)" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Color ("MainColor", Color) = (1,0,0,1)
}

SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 100
	
	// Non-lightmapped
	Pass {
		Tags { "LightMode" = "Vertex" }
		Cull Off
		Lighting Off
		AlphaTest Greater 0.5
		Blend SrcAlpha OneMinusSrcAlpha
		SetTexture [_MainTex] { 
		constantColor[_Color]
		combine texture * constant DOUBLE} 
	}
	
	// Lightmapped, encoded as dLDR
	Pass {
		Tags { "LightMode" = "VertexLM" }
		Cull Off
		Lighting Off
		BindChannels {
			Bind "Vertex", vertex
			Bind "texcoord1", texcoord0 // lightmap uses 2nd uv
			Bind "texcoord", texcoord1 // main uses 1st uv
		}
		
		SetTexture [unity_Lightmap] {
			matrix [unity_LightmapMatrix]
			combine texture
		}
		SetTexture [_MainTex] {
			combine texture * previous DOUBLE, texture * primary
		}
	}
	
	// Lightmapped, encoded as RGBM
	Pass {
		Tags { "LightMode" = "VertexLMRGBM" }
		Cull Off
		Lighting Off
		BindChannels {
			Bind "Vertex", vertex
			Bind "texcoord1", texcoord0 // lightmap uses 2nd uv
			Bind "texcoord", texcoord1 // main uses 1st uv
		}
		
		SetTexture [unity_Lightmap] {
			matrix [unity_LightmapMatrix]
			combine texture * texture alpha DOUBLE
		}
		SetTexture [_MainTex] {
			combine texture * previous QUAD, texture * primary
		}
	}	
	

}
}



