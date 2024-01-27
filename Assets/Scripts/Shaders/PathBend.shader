Shader "Custom/VertexColor"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _SwerveX("左右弯曲程度", Range(-0.003,0.003)) = 0.0
        _SwerveY("上下弯曲程度", Range(-0.003,0.003)) = 0.0
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
//        LOD 200

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

        sampler2D _MainTex;
        float _SwerveX;
        float _SwerveY;
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        void vert(inout appdata_full v)
        {
            // UNITY_INITIALIZE_OUTPUT(Input, o);
            // o.color = v.color;
            // o.uv_MainTex = v.texcoord;
            //
            // float3 WordPos = mul(unity_ObjectToWorld, v.vertex);
            // WordPos.x += pow(WordPos.z, 2) * _SwerveX;
            // WordPos.y += pow(WordPos.z, 2) * _SwerveY;
            // WordPos -= mul(unity_ObjectToWorld, float4(0, 0, 0, 1));
            // v.vertex.xyz = mul(unity_WorldToObject, WordPos).xyz;
            // // Recalculate the normal
            // float3 normalDir = float3(-_SwerveX * 2 * WordPos.z, -_SwerveY * 2 * WordPos.z, 1);
            // v.normal = normalize(mul((float3x3)unity_ObjectToWorld, normalDir));
            // o.normal = UnityObjectToWorldNormal(v.normal);


            float3 word_pos = mul(unity_ObjectToWorld, v.vertex);
            word_pos.x += pow(word_pos.z, 2) * _SwerveX;
            word_pos.y += pow(word_pos.z, 2) * _SwerveY;
            word_pos -= mul(unity_ObjectToWorld, float4(0, 0, 0, 1));
            //Convert the new world space of vertex and convert back to local space.
            float4 vertex = mul(unity_WorldToObject, word_pos);

            v.vertex = vertex;
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
            // o.Normal = IN.normal;
        }
        ENDCG
    }
    FallBack "Diffuse"
}