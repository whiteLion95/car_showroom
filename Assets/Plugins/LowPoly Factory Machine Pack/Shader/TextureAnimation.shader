Shader "Custom/TextureAnimation" {
    Properties{
        _Color("Color", Color) = (1, 1, 1, 1)
        _MainTex("Base (RGB)", 2D) = "white" {}
        _Speed("Speed", Float) = 1
    }

        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 100

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 2.0
                #pragma multi_compile_fog

                #include "UnityCG.cginc"

                struct appdata_t {
                    float4 vertex : POSITION;
                    float2 texcoord : TEXCOORD0;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                struct v2f {
                    float4 vertex : SV_POSITION;
                    float2 texcoord : TEXCOORD0;
                    UNITY_FOG_COORDS(1)
                    UNITY_VERTEX_OUTPUT_STEREO
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float _Speed;
                fixed4 _Color;

                v2f vert(appdata_t v) {
                    v2f o;
                    UNITY_SETUP_INSTANCE_ID(v);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    float2 offset = _Speed * _Time.y; // Hız bileşeni ile zamanı kullanarak offset hesapla
                    o.texcoord = TRANSFORM_TEX(v.texcoord + offset, _MainTex); // Offset'i texcoord'a uygula
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    fixed4 col = tex2D(_MainTex, i.texcoord) * _Color; // Renk bileşeni ile texture rengini çarp
                    UNITY_APPLY_FOG(i.fogCoord, col);
                    UNITY_OPAQUE_ALPHA(col.a);
                    return col;
                }
                ENDCG
            }
        }
}
