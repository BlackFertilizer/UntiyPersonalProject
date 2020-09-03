Shader "Custom/XianHe"
{
    Properties
    {
        // _Alpha ("Alpha", Range(0,1)) = 0
        // _MainTex("MainTex",2d)=""
        _MainTex ("Texture", 2D) = "white" {}//外部贴图资源的导入渠道
    }

    SubShader
    {
        Pass
        {
            Tags{"Queue" = "AlphaTest"}
            Blend SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _MainTex_ST;

            struct a2v
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord: TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;//贴图坐标
                float4 pos: SV_POSITION;
                //fixed4 color : COLOR0;

            };

            v2f vert(a2v v)
            { 
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                //o.color = fixed4(1,0,0,0);
                return o;
            }


            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 color = tex2D(_MainTex, i.uv);
                if(color.a < 0.1)
                {
                    discard;
                }

                return color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"

}
