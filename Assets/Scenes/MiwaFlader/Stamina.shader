Shader "Custom/StaminaGauge"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}   // ベース画像
        _FillColor("Fill Color", Color) = (0,1,0,1) // ゲージの色 (緑)
        _BackColor("Back Color", Color) = (0.2,0.2,0.2,1) // 背景色 (灰)
        _FillAmount("Fill Amount", Range(0,1)) = 1.0 // スタミナ割合
    }
        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _FillColor;
                float4 _BackColor;
                float _FillAmount;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // 背景色
                    fixed4 col = _BackColor;

                // uv.x が FillAmount 以下なら塗りつぶす
                if (i.uv.x <= _FillAmount)
                {
                    col = _FillColor;
                }

                return col;
            }
            ENDCG
        }
        }
}
