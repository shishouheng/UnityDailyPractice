Shader "Custom/Toon"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _RampTex("Ramp",2D)="white"{}
        _ToonShadingLevels("_ToonShadingLevels",Range(2,10))=10
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Toon2

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _RampTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        float _ToonShadingLevels;
        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf(Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
        }

        fixed4 LightingToon(SurfaceOutput s,fixed3 lightDir,fixed atten)
        {
            half NdotL = dot(s.Normal, lightDir);
            NdotL = tex2D(_RampTex,fixed2(NdotL, 0.5));
            fixed4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * NdotL * atten;
            c.a = s.Alpha;
            return c;
        }

        half4 LightingToon2(SurfaceOutput s, fixed3 lightDir, half3 viewDir, half atten)
        {
            half NdotL = dot(s.Normal, lightDir);
            half toon = floor(NdotL * _ToonShadingLevels) / (_ToonShadingLevels - 0.5);
            half4 c;
            c.rgb = s.Albedo * _LightColor0 * toon * atten;
            c.a = s.Alpha;
            return c;
        }
        ENDCG
    }
    FallBack "Diffuse"
}