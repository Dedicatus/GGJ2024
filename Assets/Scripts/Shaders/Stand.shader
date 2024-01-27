// Upgrade NOTE: replaced 'defined FOG_COMBINED_WITH_WORLD_POS' with 'defined (FOG_COMBINED_WITH_WORLD_POS)'

Shader "SurfaceTest/Surf2Vert"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Main Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
        }

        // ------------------------------------------------------------
        // Surface shader code generated out of a CGPROGRAM block:


        // ---- forward rendering base pass:
        Pass
        {
            Name "FORWARD"
            Tags
            {
                "LightMode" = "ForwardBase"
            }

            CGPROGRAM
            // compile directives
            #pragma vertex vert_surf
            #pragma fragment frag_surf
            #pragma multi_compile_instancing
            #pragma multi_compile_fog
            #pragma multi_compile_fwdbase
            #include "HLSLSupport.cginc"
            #define UNITY_INSTANCED_LOD_FADE
            #define UNITY_INSTANCED_SH
            #define UNITY_INSTANCED_LIGHTMAPSTS
            #include "UnityShaderVariables.cginc"
            #include "UnityShaderUtilities.cginc"
            // -------- variant for: <when no other keywords are defined>
            #if !defined(INSTANCING_ON)

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            #define INTERNAL_DATA
            #define WorldReflectionVector(data,normal) data.worldRefl
            #define WorldNormalVector(data,normal) normal


            half4 LightingSimpleLambert(SurfaceOutput s, half3 lightDir, half atten)
            {
                half NdotL = dot(s.Normal, lightDir);
                half4 c;
                c.rgb = s.Albedo * _LightColor0.rgb * max(0, NdotL);
                c.a = s.Alpha;
                return c;
                //   return fixed4(0,0,0,1);
            }

            struct Input
            {
                float2 uv_MainTex;
            };

            sampler2D _MainTex;
            fixed4 _Color;

            void surf(Input IN, inout SurfaceOutput o)
            {
                o.Albedo = tex2D(_MainTex, IN.uv_MainTex) * _Color;
                // o.Albedo = fixed4(0,1,0,1);
            }

            // high-precision fragment shader registers:
            #ifndef UNITY_HALF_PRECISION_FRAGMENT_SHADER_REGISTERS
            struct v2f_surf
            {
                UNITY_POSITION(pos);
                float2 pack0 : TEXCOORD0; // _MainTex
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
                fixed3 vlight : TEXCOORD3; // ambient/SH/vertexlights
                UNITY_FOG_COORDS(4)
                UNITY_SHADOW_COORDS(5)
                #if SHADER_TARGET >= 30
  float4 lmap : TEXCOORD6;
                #endif
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };
            #endif
            // with lightmaps:

            float4 _MainTex_ST;

            // vertex shader
            v2f_surf vert_surf(appdata_full v)
            {
                //   UNITY_SETUP_INSTANCE_ID(v);
                v2f_surf o;
                //   UNITY_INITIALIZE_OUTPUT(v2f_surf,o);
                //   UNITY_TRANSFER_INSTANCE_ID(v,o);
                //   UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.pack0.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos.xyz = worldPos;
                o.worldNormal = worldNormal;

                // SH/ambient and vertex lights
                #ifndef LIGHTMAP_ON
                #if UNITY_SHOULD_SAMPLE_SH && !UNITY_SAMPLE_FULL_SH_PER_PIXEL
    float3 shlight = ShadeSH9 (float4(worldNormal,1.0));
    o.vlight = shlight;
                #else
                o.vlight = 0.0;
                #endif
                #endif // !LIGHTMAP_ON

                //   UNITY_TRANSFER_LIGHTING(o,v.texcoord1.xy); // pass shadow and, possibly, light cookie coordinates to pixel shader
                //   #ifdef FOG_COMBINED_WITH_TSPACE
                //     UNITY_TRANSFER_FOG_COMBINED_WITH_TSPACE(o,o.pos); // pass fog coordinates to pixel shader
                //   #elif defined (FOG_COMBINED_WITH_WORLD_POS)
                //     UNITY_TRANSFER_FOG_COMBINED_WITH_WORLD_POS(o,o.pos); // pass fog coordinates to pixel shader
                //   #else
                //     UNITY_TRANSFER_FOG(o,o.pos); // pass fog coordinates to pixel shader
                //   #endif
                return o;
            }

            // fragment shader
            fixed4 frag_surf(v2f_surf IN) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(IN);
                // prepare and unpack data
                Input surfIN;
                UNITY_INITIALIZE_OUTPUT(Input, surfIN);
                surfIN.uv_MainTex.x = 1.0;
                surfIN.uv_MainTex = IN.pack0.xy;
                float3 worldPos = IN.worldPos.xyz;
                #ifndef USING_DIRECTIONAL_LIGHT
                fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
                #else
    fixed3 lightDir = _WorldSpaceLightPos0.xyz;
                #endif
                #ifdef UNITY_COMPILER_HLSL
                SurfaceOutput o = (SurfaceOutput)0;
                #else
    SurfaceOutput o;
                #endif
                o.Albedo = 0.0;
                o.Emission = 0.0;
                o.Specular = 0.0;
                o.Alpha = 0.0;
                o.Gloss = 0.0;
                fixed3 normalWorldVertex = fixed3(0, 0, 1);
                //  return fixed4(0.5* normalize(IN.worldNormal)+0.5, 1);
                o.Normal = IN.worldNormal;
                normalWorldVertex = IN.worldNormal;

                surf(surfIN, o);

                UNITY_LIGHT_ATTENUATION(atten, IN, worldPos)
                fixed4 c = 0;

                #ifndef LIGHTMAP_ON
                c.rgb += o.Albedo * IN.vlight;
                #endif

                #ifndef LIGHTMAP_ON
                c += LightingSimpleLambert(o, lightDir, atten);
                //   return c;
                #else
    c.a = o.Alpha;
                #endif

                UNITY_APPLY_FOG(_unity_fogCoord, c); // apply fog
                UNITY_OPAQUE_ALPHA(c.a);
                return c;
            }
            #endif
            ENDCG

        }

    }
}