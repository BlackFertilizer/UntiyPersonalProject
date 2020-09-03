Shader "Customs/Animals"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}//外部贴图资源的导入渠道
        _ShadowColor("ShadowColor",COLOR) = (0.0,0.0,0.0,1.0)
        _ShadowHeight("ShadowHeight",Range(0,5.0)) = 1.0
        _ShadowIntensity("ShadowIntensity",Range(0,1)) = 1.0
        _ShadowFalloff("ShadowFallOff", Range(0,1)) = 0.0
    }

    SubShader
    {
        Tags{ "RenderType" = "Opaque" "Queue" = "Geometry+10" }

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
            };

            v2f vert(a2v v)
            { 
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
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

                
        
        //阴影pass
        Pass
        {
            Name "PlanarShadowPass"
            Tags{"LightMode" ="ForwardBase" }

            //用使用模板测试以保证alpha显示正确
            Stencil
            {
                Ref 0
                Comp equal
                Pass incrWrap
                Fail keep
                ZFail keep
            }

            //透明混合模式
            Blend SrcAlpha OneMinusSrcAlpha

            //关闭深度写入
            ZWrite off

            //深度稍微偏移防止阴影与地面穿插
            Offset -1 , 0

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_fwdbase
            
            

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            float4 _LightDir;
            float4 _ShadowColor;
            float _ShadowHeight;
            float _ShadowIntensity;
            float _ShadowFalloff;

            float3 ShadowProjectPos(float4 vertPos)
            {
                float3 shadowPos;
                _LightDir.xyz = _WorldSpaceLightPos0.xyz;
                //_ShadowColor = fixed4(0.0,0.0,0.0,1.0);
                //_ShadowFalloff = 1.0;

                //得到顶点的世界空间坐标
                float3 worldPos = mul(unity_ObjectToWorld , vertPos).xyz;
                

                //灯光方向
                float3 lightDir = normalize(_LightDir.xyz);

                //阴影的世界空间坐标（低于地面的部分不做改变）
                shadowPos.y = _ShadowHeight;//min( , 0);
                shadowPos.xz = worldPos .xz - lightDir.xz * (max(0 , worldPos .y) - _ShadowHeight) / lightDir.y; 

                return shadowPos;
            }

            v2f vert (appdata v)
            {
                v2f o;

                //得到阴影的世界空间坐标
                float3 shadowPos = ShadowProjectPos(v.vertex);

                //转换到裁切空间
                o.vertex = UnityWorldToClipPos(shadowPos);

                //得到中心点世界坐标
                float3 center =float3( unity_ObjectToWorld[0].w , _ShadowHeight , unity_ObjectToWorld[2].w);
                //float3 center = float3(unity_ObjectToWorld[0].w, unity_ObjectToWorld[1].w, unity_ObjectToWorld[2].w)/50.0;
                //计算阴影衰减 saturate 返回 0~1 
                float falloff = 1 - saturate(distance(shadowPos , center) * _ShadowFalloff);

                // //阴影颜色
                 //o.color = fixed4(center.x, center.y,center.z,1.0);
                 o.color = _ShadowColor;
                 
                 o.color.a *= falloff;
                 o.color.a *= _ShadowIntensity;
                 //o.color.a = 0.6;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }
    }

}
