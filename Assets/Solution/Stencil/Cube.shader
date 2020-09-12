Shader "Custom/Cube"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
        //----add----
        [HideInInspector] _ID ("_ID",int) = 1
        [HideInInspector] _StencilComp ("_StencilComp",Float) = 8
        //----add----
	}
	SubShader
	{
        Tags { "RenderType"="Opaque" "Queue"="Geometry"}
        Pass
		{
            //----add----
            Stencil
            {
                Ref [_ID]
                Comp [_StencilComp]
                Pass keep
            }
            //----add----

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;

			};

			struct v2f
			{
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
			};


            sampler2D _MainTex;
            float4 _MainTex_ST;

			v2f vert (appdata v)
			{
	            v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
			}

			fixed4 frag (v2f IN) : SV_Target
			{
                return tex2D(_MainTex, IN.texcoord);
			}
			ENDCG
		}
	}
}