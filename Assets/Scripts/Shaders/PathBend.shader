//Shader "Custom/PathBend"
//{
//    Properties
//    {
//        _MainTex ("颜色纹理", 2D) = "white" {}
//        _SwerveX("左右弯曲程度", Range(-0.003,0.003)) = 0.0
//        _SwerveY("上下弯曲程度", Range(-0.003,0.003)) = 0.0
//        _Glossiness ("Smoothness", Range(0,1)) = 0.5
//        _Metallic ("Metallic", Range(0,1)) = 0.0
//    }
//    SubShader
//    {
//        Tags
//        {
//            "RenderType"="Opaque"
//        }
//        LOD 200
//
//        Pass
//        {
//            CGPROGRAM
//            #pragma vertex vert
//            #pragma fragment frag
//            // make fog work
//            #pragma multi_compile_fog
//
//
//            #include "UnityCG.cginc"
//
//            struct appdata
//            {
//                float4 vertex : POSITION;
//
//                //获取模型第一套UV
//                float2 uv : TEXCOORD0;
//                float4 normal: NORMAL;
//            };
//
//            struct v2f
//            {
//                float2 uv : TEXCOORD0;
//                float4 vertex : SV_POSITION;
//                float4 normal: NORMAL;
//                UNITY_FOG_COORDS(4)
//            };
//
//            //颜色纹理
//            sampler2D _MainTex;
//            float _SwerveX;
//            float _SwerveY;
//            half _Glossiness;
//            half _Metallic;
//
//            v2f vert(appdata v)
//            {
//                v2f o;
//
//                o.uv = v.uv;
//
//                //获取模型的空间坐标
//                float3 WordPos = mul(unity_ObjectToWorld, v.vertex);
//
//                //左右左右坐标作为弯道 
//                //依据Z坐标求平方获取弯曲曲线，越远离世界坐标原点，弯曲效果越明显。
//                //最后乘以左右弯道弯曲方向，和弯曲强度
//                WordPos.x += pow(WordPos.z, 2) * _SwerveX;
//                //方法与上面相同，改变Y轴，获得上下坡效果
//                WordPos.y += pow(WordPos.z, 2) * _SwerveY;
//
//                //修正模型位置，WordPos 不包含物体自身的空间位移
//                WordPos -= mul(unity_ObjectToWorld, float4(0, 0, 0, 1));
//
//                //修改世界顶点转回物体自身顶点。
//                v.vertex = mul(unity_WorldToObject, WordPos);
//
//                //转换为裁切空间
//                o.vertex = UnityObjectToClipPos(v.vertex);
//
//                o.normal = v.normal;
//                UNITY_TRANSFER_FOG(o, o.vertex);
//                return o;
//            }
//
//            // fixed4 frag(v2f i) : SV_Target
//            // {
//            //     //获取颜色贴图
//            //     fixed4 col = tex2D(_MainTex, i.uv);
//            //
//            //     // apply fog
//            //     UNITY_APPLY_FOG(i.fogCoord, col);
//            //
//            //     // Calculate normal
//            //     float3 normal = normalize(i.normal);
//            //
//            //     // Calculate light direction
//            //     float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
//            //
//            //     // Calculate diffuse lighting
//            //     float diff = max(0, dot(normal, lightDir));
//            //
//            //     // Apply lighting to color
//            //     col.rgb *= diff;
//            //
//            //     return col;
//            // }
//            fixed4 frag(v2f i) : SV_Target
//            {
//                // Albedo comes from a texture tinted by color
//                fixed4 c = tex2D(_MainTex, i.uv) * _Color;
//
//                // Calculate normal
//                float3 normal = normalize(i.normal);
//
//                // Initialize lighting
//                float3 lighting = float3(0, 0, 0);
//
//                // Loop over all lights
//                for (int j = 0; j < _LightColor0_array.Length; j++)
//                {
//                    // Calculate light direction
//                    float3 lightDir = normalize(_WorldSpaceLightPos0_array[j].xyz);
//
//                    // Calculate diffuse lighting
//                    float diff = max(0, dot(normal, lightDir));
//
//                    // Add light contribution to lighting
//                    lighting += _LightColor0_array[j] * diff;
//                }
//
//                // Apply lighting to color
//                c.rgb *= lighting;
//
//                // Metallic and smoothness come from slider variables
//                c.rgb *= lerp(1.0, c.rgb, _Metallic);
//                c.a *= _Glossiness;
//
//                return c;
//            }
//            ENDCG
//        }
//    }
//}


Shader "Custom/VertexColor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _SwerveX ("SwerveX", Float) = 0
        _SwerveY ("SwerveY", Float) = 0
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

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

        void surf(Input IN, inout SurfaceOutput o)
        {
            // fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
            // o.Albedo = c.rgb;
            // o.Alpha = c.a;
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            // o.Metallic = _Metallic;
            // o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            o.Normal = IN.normal;
        }

        // void vert(inout appdata_full v, out Input o)
        // {
        //     UNITY_INITIALIZE_OUTPUT(Input, o);
        //     o.color = v.color;
        //     o.uv_MainTex = v.texcoord;
        //     o.normal = v.normal;
        //
        //     float3 WordPos = mul(unity_ObjectToWorld, v.vertex);
        //     WordPos.x += pow(WordPos.z, 2) * _SwerveX;
        //     WordPos.y += pow(WordPos.z, 2) * _SwerveY;
        //     WordPos -= mul(unity_ObjectToWorld, float4(0, 0, 0, 1));
        //     v.vertex = mul(unity_WorldToObject, WordPos);
        //     o.vertex = UnityObjectToClipPos(v.vertex);
        // }
        // void vert (inout appdata_full v, out Input o) {
        //     UNITY_INITIALIZE_OUTPUT(Input, o);
        //     o.color = v.color;
        //     o.uv_MainTex = v.texcoord;
        //     o.normal = v.normal;
        //
        //     float3 WordPos = mul(unity_ObjectToWorld, v.vertex);
        //     WordPos.x += pow(WordPos.z, 2) * _SwerveX;
        //     WordPos.y += pow(WordPos.z, 2) * _SwerveY;
        //     WordPos -= mul(unity_ObjectToWorld, float4(0, 0, 0, 1));
        //     v.vertex.xyz = mul(unity_WorldToObject, WordPos).xyz;
        // }

        // void vert(inout appdata_full v, out Input o)
        // {
        //     UNITY_INITIALIZE_OUTPUT(Input, o);
        //     o.color = v.color;
        //     o.uv_MainTex = v.texcoord;
        //
        //     float3 WordPos = mul(unity_ObjectToWorld, v.vertex);
        //     WordPos.x += pow(WordPos.z, 2) * _SwerveX;
        //     WordPos.y += pow(WordPos.z, 2) * _SwerveY;
        //     WordPos -= mul(unity_ObjectToWorld, float4(0, 0, 0, 1));
        //     v.vertex.xyz = mul(unity_WorldToObject, WordPos).xyz;
        //
        //     // Recalculate the normal
        //     float3 normalDir = float3(_SwerveX * 2 * WordPos.z, _SwerveY * 2 * WordPos.z, 1);
        //     v.normal = mul((float3x3)unity_ObjectToWorld, normalDir);
        //     o.normal = UnityObjectToWorldNormal(v.normal);
        // }

        // void vert(inout appdata_full v, out Input o)
        // {
        //     UNITY_INITIALIZE_OUTPUT(Input, o);
        //     o.color = v.color;
        //     o.uv_MainTex = v.texcoord;
        //
        //     float3 WordPos = mul(unity_ObjectToWorld, v.vertex);
        //     WordPos.x += pow(WordPos.z, 2) * _SwerveX;
        //     WordPos.y += pow(WordPos.z, 2) * _SwerveY;
        //     WordPos -= mul(unity_ObjectToWorld, float4(0, 0, 0, 1));
        //     v.vertex.xyz = mul(unity_WorldToObject, WordPos).xyz;
        //
        //     // Recalculate the normal
        //     float3 normalDir = float3(_SwerveX * 2 * WordPos.z, _SwerveY * 2 * WordPos.z, 1);
        //     v.normal = mul((float3x3)unity_ObjectToWorld, normalDir);
        //     o.normal = UnityObjectToWorldNormal(v.normal);
        // }

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
        ENDCG
    }
    FallBack "Diffuse"
}

//Shader "Custom/VertexColor"
//{
//    Properties
//    {
//        _MainTex ("Texture", 2D) = "white" {}
//        _SwerveX ("SwerveX", Float) = 0
//        _SwerveY ("SwerveY", Float) = 0
//        _Glossiness ("Glossiness", Range(0, 1)) = 0.5
//        _Metallic ("Metallic", Range(0, 1)) = 0.5
//    }
//    SubShader
//    {
//        Tags
//        {
//            "RenderType"="Opaque"
//        }
//        LOD 100
//
//        CGPROGRAM
//        #pragma surface surf Standard vertex:vert
//
//        struct Input
//        {
//            float2 uv_MainTex;
//            fixed4 color : COLOR;
//            float3 worldNormal;
//            INTERNAL_DATA
//        };
//
//        sampler2D _MainTex;
//        half _Glossiness;
//        half _Metallic;
//        float _SwerveX;
//        float _SwerveY;
//
//        void surf(Input IN, inout SurfaceOutputStandard o)
//        {
//            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
//            o.Albedo = c.rgb;
//            o.Metallic = _Metallic;
//            o.Smoothness = _Glossiness;
//            o.Alpha = c.a;
//            o.Normal = IN.worldNormal;
//        }
//
//        void vert(inout appdata_full v, out Input o)
//        {
//                UNITY_INITIALIZE_OUTPUT(Input, o);
//            o.color = v.color;
//            o.uv_MainTex = v.texcoord;
//
//            float3 WordPos = mul(unity_ObjectToWorld, v.vertex);
//            WordPos.x += pow(WordPos.z, 2) * _SwerveX;
//            WordPos.y += pow(WordPos.z, 2) * _SwerveY;
//            WordPos -= mul(unity_ObjectToWorld, float4(0, 0, 0, 1));
//            v.vertex.xyz = mul(unity_WorldToObject, WordPos).xyz;
//
//            // Recalculate the normal
//            float3 normalDir = float3(-_SwerveX * 2 * WordPos.z, -_SwerveY * 2 * WordPos.z, 1);
//            v.normal = normalize(mul((float3x3)unity_ObjectToWorld, normalDir));
//            o.worldNormal = UnityObjectToWorldNormal(v.normal);
//        }
//        ENDCG
//    }
//    FallBack "Diffuse"
//}