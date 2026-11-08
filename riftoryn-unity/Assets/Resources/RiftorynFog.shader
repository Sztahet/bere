Shader "Riftoryn/LowFog"
{
    Properties { _Color ("Fog color", Color) = (0.68, 0.57, 0.76, 0.29) }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            fixed4 _Color;
            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float4 position : SV_POSITION; float2 uv : TEXCOORD0; float3 world : TEXCOORD1; };
            v2f vert(appdata v)
            {
                v2f o;
                o.position=UnityObjectToClipPos(v.vertex);
                o.world=mul(unity_ObjectToWorld,v.vertex).xyz;
                o.uv=v.uv;
                return o;
            }
            float hash(float2 p) { return frac(sin(dot(p,float2(127.1,311.7)))*43758.5453); }
            float noise(float2 p)
            {
                float2 i=floor(p), f=frac(p);
                f=f*f*(3.0-2.0*f);
                return lerp(lerp(hash(i),hash(i+float2(1,0)),f.x),
                            lerp(hash(i+float2(0,1)),hash(i+1),f.x),f.y);
            }
            fixed4 frag(v2f i) : SV_Target
            {
                float2 cloud=i.world.xz*0.48+float2(_Time.y*0.035,-_Time.y*0.022)+i.world.y*0.2;
                float density=0.65*noise(cloud)+0.35*noise(cloud*2.2+float2(-_Time.y*0.025,0));
                float edge=saturate(1.0-length((i.uv-0.5)*2.0));
                edge=edge*edge*(3.0-2.0*edge);
                return fixed4(_Color.rgb,_Color.a*edge*smoothstep(0.18,0.8,density));
            }
            ENDCG
        }
    }
}
