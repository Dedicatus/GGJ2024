Shader "Custom/CurvedSurfaceShader"
{
    Properties
    {
        _Color ("Colour", Color) = (1,1,1,1)
        _LightColor ("Light", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        //        _BendAmount ("Bend Amount", Vector) = (0,0,0)
        //        _BendOrigin ("Bend Origin", Vector) = (0,0,0)
        //        _BendFallOff ("Bend Falloff", Float) = 0
        //        _BendFallOffStr ("Bend Falloff Strength", Float) = 1
        _SwerveX("左右弯曲程度", Range(-0.003,0.003)) = 0.0
        _SwerveY("上下弯曲程度", Range(-0.003,0.003)) = 0.0
        _SinSpeed("上下弯曲速度", Range(0.0, 1.0)) = 0.15
        _SinFrequency("上下弯曲频率", Range(0.0, 1.0)) = 0.15
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }

        CGPROGRAM
        #pragma target 3.0

        #include "UnityCG.cginc"

        //Physically based Standard lighting model, and enable shadows on all light types.
        #pragma surface surf Standard fullforwardshadows addshadow
        //Vertex needed to generate curve effect.
        #pragma vertex vert


        struct Input
        {
            float2 uv_MainTex;
        };

        // float3 _BendAmount;
        // float3 _BendOrigin;
        // float _BendFallOff;
        // float _BendFallOffStr;

        sampler2D _MainTex;
        float _SwerveX;
        float _SwerveY;
        half _Glossiness;
        half _Metallic;
        half _SinSpeed;
        half _SinFrequency;
        fixed4 _Color;
        fixed4 _LightColor;
        #define time _Time.y*0.5

        void vert(inout appdata_full v)
        {
            //Getting world space location of a particular vertex.
            float4 word_pos = mul(unity_ObjectToWorld, v.vertex);
            //Calculating the distance between the vertex position and where the _BendOrigin is.
            // float dist = length(word_pos.xyz - _BendOrigin.xyz);
            // //Distance value is prevented from going below 0, This prevents undefined behaviour when using power function (negative values don't work with power).
            // dist = max(0, dist - _BendFallOff);
            //
            // //Distance value is raised to _BendFallOffStr value, which defines the steepness of the curve itself.
            // dist = pow(dist, _BendFallOffStr);
            //_BendAmount should be dependent on the distance value so as to have a falloff curve, So we simply multiply it with distance and add to 'world'.
            // word_pos.xyz += dist * _BendAmount;
            word_pos.x += pow(word_pos.z, 2) * _SwerveX;
            word_pos.y += pow(word_pos.z, 2) * _SwerveY;
            word_pos.y += sin(word_pos.z * _SinFrequency - time) * _SinSpeed;

            //Convert the new world space of vertex and convert back to local space.
            float4 vertex = mul(unity_WorldToObject, word_pos);

            v.vertex = vertex;
        }


        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by colour
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            if (c.x == 1 && c.y == 0 && c.z == 1)
            {
                o.Emission = _LightColor;
            }
            c *= _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}