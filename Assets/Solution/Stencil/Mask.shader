Shader "Custom/Mask"
{
    Properties
    {
        [HideInInspector] _ID("_ID",int) = 1
    }
        SubShader
    {
         Pass
         {
             Tags{ "RenderType" = "Opaque" "Queue" = "Geometry-1" }
             ColorMask 0
             ZWrite off
             ZTest off
             Stencil
             {
                 Ref[_ID]
                 Comp always
                 Pass replace //替换相同ID模板像素
             }
             CGINCLUDE
             struct appdata {
                 float4 vertex : POSITION;
             };
             struct v2f {
                 float4 vertex : SV_POSITION;
             };
             v2f vert(appdata v) {
                 v2f o;
                 return o;
             }
             half4 frag(v2f i) : SV_Target{
                 return 0;
             }
             ENDCG
         }
    }
}