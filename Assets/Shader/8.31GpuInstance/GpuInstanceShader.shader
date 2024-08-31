Shader "Unlit/GpuInstanceShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            //1.启用支持GPU Instance的shader变体
            #pragma multi_compile_instancing

            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float4,_Color)
	      	    UNITY_DEFINE_INSTANCED_PROP(float, _Phi)
            UNITY_INSTANCING_BUFFER_END(Props)

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;

                //2.在输入结构体中声明实例ID输入
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                //3.在输出结构体中声明实例ID输出
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                //4.初始化并设置当前顶点对应的实例id，将实例id与顶点关联起
                UNITY_SETUP_INSTANCE_ID(v);
                //5.将实例id从顶点着色器传递到片元着色器
                UNITY_TRANSFER_INSTANCE_ID(v,o);

                float phi = UNITY_ACCESS_INSTANCED_PROP(Props, _Phi);
                v.vertex = v.vertex + sin(_Time.y + phi);
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //6.在片元着色器中设置实例id，使得片元着色器能够根据实例id读取每个实例的特定数据：颜色、矩阵等
                UNITY_SETUP_INSTANCE_ID(i);
                //得到由CPU设置的颜色
                float4 col= UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
                return col;
            }
            ENDCG
        }
    }
}
