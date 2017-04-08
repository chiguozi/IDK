// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "X1/TransparentDetailCharacter" {
	Properties {
		_MainTex ("Main Texture", 2D) = "white" {}
		_HighLightTex ("高光贴图", 2D) = "white" {}
		_LightColor("光照颜色",Color) = (0.3, 0.4, 0.7, 1.0)
		_DIRLightColor("平行光颜色", Color) = (0.3, 0.4, 0.7, 1.0)
		_LightIntensity("光照强度",Float) = 0
		_DIRLightIntensity("平行光照强度",Float) = 0
		_Ambient("环境光亮度", Float) = 0
		_RimColor("描边颜色",Color) = (0.31, 0.3, 0.5, 1.0)
		_RimIntensity("描边强度",Float) = 0.35
		_RimFalloff("描边衰减(1-10)",Float) = 2
		_LightPos("光源位置",Vector) = (1,-1,1,0)
		_HLIntensity("高光强度",Float) = 1.0
		//_SSSColor("SSS颜色",Color) = (0.3, 0.4, 0.7, 1.0)
	}
	SubShader {
	Tags 
	{
		"RenderType"="Transparent"
		"IgnoreProjector"="True"
		"Queue" = "Transparent"
	}
	Pass {
		Lighting Off
		Zwrite On
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"
		struct appdata_t {
				fixed4 vertex : POSITION;
				fixed2 texcoord : TEXCOORD0;
				fixed3 normal : NORMAL;
			};

			struct v2f {
				fixed4 vertex : POSITION;
				fixed2 texcoord : TEXCOORD0;
				fixed3 camdir : TEXCOORD1;
				fixed3 normal : TEXCOORD2;
				fixed3 pos : TEXCOORD3;
			};
  
			uniform sampler2D _MainTex;
			uniform sampler2D _HighLightTex;   
			uniform fixed4 _LightColor;
			uniform fixed4 _DIRLightColor;
			//uniform fixed4 _SSSColor;
			uniform fixed _LightIntensity;
			uniform fixed _DIRLightIntensity;
			uniform fixed4 _RimColor;
			uniform fixed _RimIntensity;
			uniform fixed4 _LightPos;
			uniform fixed _HLIntensity;
			uniform fixed _Ambient;
			uniform fixed _RimFalloff;
			v2f vert (appdata_t v)
			{
				v2f o;
				fixed3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				fixed3 N = normalize((mul(fixed4(v.normal, 0), unity_WorldToObject).xyz));
				fixed3 I = normalize(_WorldSpaceCameraPos - worldPos);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				o.pos = worldPos;
				o.normal = N;
				o.camdir = I;
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				fixed4 col;

				fixed4 texcolor = tex2D(_MainTex, i.texcoord);

				//点光源
				fixed3 vec = _LightPos.xyz - i.pos;
				fixed3 lightDir = normalize(vec);
				fixed dis = saturate(1/length(vec));
				fixed lightValue = dot(lightDir,i.normal);
				col = texcolor * _LightColor * saturate(lightValue) * _LightIntensity * dis * dis;

				//fixed sssValue = 1 - abs(lightValue); 
				//col += sssValue * _SSSColor * _LightIntensity * dis; 

				

				//环境光
				col += _Ambient * texcolor;

				//平行光
				col += saturate(dot(fixed3(0,1,0),i.normal)) * _DIRLightColor * _DIRLightIntensity * texcolor;

				//rim
				fixed rim = 1 - abs(dot(i.camdir,i.normal));
				col += _RimColor * pow(rim,_RimFalloff) * _RimIntensity;

				//反射
				fixed3 I = normalize(_WorldSpaceCameraPos - i.pos);
				fixed3 reflectDir = normalize(reflect(normalize(vec),i.normal));
				fixed highLightIntensity = saturate(dot(I,reflectDir));
				highLightIntensity = highLightIntensity * highLightIntensity;
				highLightIntensity = highLightIntensity * highLightIntensity;
				fixed4 highLightcolor = tex2D(_HighLightTex, i.texcoord);
				col += highLightIntensity * highLightcolor * _HLIntensity  * _LightColor * _LightIntensity * dis;
				
				// col.a = texcolor.a;
				// return saturate(col);
				
				col = saturate(col);
				col.a = texcolor.a;
				return col;
			}
		ENDCG
		}
	}
	Fallback "VertexLit"
}

