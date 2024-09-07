Shader "Custom/PackedTextureShader"
{
    Properties
    {
        _MainTint("Diffuse Tint",Color)=(1,1,1,1)
        _ColorA("Terrain Color A",Color)=(1,1,1,1)
        _ColorB("Terrain Color B",Color)=(1,1,1,1)
        _RTexture("Red Channel Texture",2D)=""{}
        _GTexture("Green Channel Texture",2D)=""{}
        _BTexture("Blue Channel Texture",2D)=""{}
        _ATexture("Alpha Channel Texture",2D)=""{}
        _BlendTex("Blend Texture",2D)=""{}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Lambert 

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        

        struct Input
        {
            float2 uv_BlendTex;
        };

        float4 _MainTint;
        float4 _ColorA;
        float4 _ColorB;
        sampler2D _RTexture;
        sampler2D _GTexture;
        sampler2D _BTexture;
        sampler2D _ATexture;
        sampler2D _BlendTex;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            float4 blendData=tex2D(_BlendTex,IN.uv_BlendTex);
            float4 rTexData=tex2D(_RTexture,IN.uv_BlendTex);
            float4 gTexData=tex2D(_GTexture,IN.uv_BlendTex);
            float4 bTexData=tex2D(_BTexture,IN.uv_BlendTex);
            float4 aTexData=tex2D(_ATexture,IN.uv_BlendTex);
            float4 finalColor;
            finalColor=lerp(rTexData,gTexData,blendData.g);
            finalColor=lerp(finalColor,bTexData,blendData.b);
            finalColor=lerp(finalColor,aTexData,blendData.a);
            finalColor.a=1.0;
            float4 terrainLayers=lerp(_ColorA,_ColorB,blendData.g);
            finalColor*=terrainLayers;
            finalColor=saturate(finalColor);
            o.Albedo=finalColor*_MainTint.rgb;
            o.Alpha=finalColor.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
