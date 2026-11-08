Shader "Riftoryn/SparkPreviewOutline"
{
    Properties { _OutlinePixels ("Outline pixels", Range(0,4)) = 1.35  _Color ("Outline", Color) = (0.10,0.035,0.025,1) }
    SubShader
    {
        Tags { "Queue"="Geometry+1" "RenderType"="Opaque" }
        Pass
        {
            Cull Front ZWrite Off ZTest LEqual
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            float _OutlinePixels; fixed4 _Color;
            struct appdata { float4 vertex : POSITION; float3 normal : NORMAL; };
            struct v2f { float4 pos : SV_POSITION; };
            v2f vert(appdata v) {
                v2f o; o.pos=UnityObjectToClipPos(v.vertex);
                float3 viewNormal=mul((float3x3)UNITY_MATRIX_IT_MV,v.normal);
                float2 direction=normalize(viewNormal.xy+float2(.0001,.0001));
                o.pos.xy+=direction*(_OutlinePixels*2.0/_ScreenParams.xy)*o.pos.w;
                return o;
            }
            fixed4 frag(v2f i) : SV_Target { return _Color; }
            ENDCG
        }
    }
}
