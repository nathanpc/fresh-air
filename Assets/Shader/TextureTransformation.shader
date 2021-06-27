Shader "Custom/TextureTransformation" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Original Texture", 2D) = "white" {}
        _TransTex("Transformed Texture", 2D) = "gray" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.0
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Transformation("Transformation", Range(0,1)) = 0.0
    }

    SubShader {
        // Make sure we can do transparency.
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 300
        Blend SrcAlpha OneMinusSrcAlpha  // Use blend for alpha values.

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types.
        #pragma surface surf Standard fullforwardshadows
        // Use shader model 3.0 target, to get nicer looking lighting.
        #pragma target 3.0

        struct Input {
            float2 uv_MainTex;
            float2 uv_TransTex;
        };

        // Turn our properties into variables.
        sampler2D _MainTex;
        sampler2D _TransTex;
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        half _Transformation;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        // Calculate the surface properties.
        void surf (Input IN, inout SurfaceOutputStandard o) {
            // Get textures.
            fixed4 colMain = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            fixed4 colTrans = tex2D(_TransTex, IN.uv_TransTex) * _Color;

            // Calculate how much each texture will weigh in the transformation.
            o.Albedo = (colMain.rgb * (1 - _Transformation)) + (colTrans.rgb * _Transformation);

            // Apply all the other "standard" effects.
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = _Color.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
