Shader "Custom/VertexColor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _SwerveX("左右弯曲程度", Range(-0.003,0.003)) = 0.0
        _SwerveY("上下弯曲程度", Range(-0.003,0.003)) = 0.0
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert

        struct Input
        {
            float2 uv_MainTex;
            fixed4 color : COLOR;
            float3 normal : NORMAL;
        };

        sampler2D _MainTex;
        float _SwerveX;
        float _SwerveY;
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.color = v.color;
            o.uv_MainTex = v.texcoord;

            float3 WordPos = mul(unity_ObjectToWorld, v.vertex);
            WordPos.x += pow(WordPos.z, 2) * _SwerveX;
            WordPos.y += pow(WordPos.z, 2) * _SwerveY;
            WordPos -= mul(unity_ObjectToWorld, float4(0, 0, 0, 1));
            v.vertex.xyz = mul(unity_WorldToObject, WordPos).xyz;
            // Recalculate the normal
            float3 normalDir = float3(-_SwerveX * 2 * WordPos.z, -_SwerveY * 2 * WordPos.z, 1);
            v.normal = normalize(mul((float3x3)unity_ObjectToWorld, normalDir));
            o.normal = UnityObjectToWorldNormal(v.normal);
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
            o.Normal = IN.normal;
        }
        ENDCG
    }
    FallBack "Diffuse"
}