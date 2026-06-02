Shader "UI/IrisWipe"
{
    Properties
    {
        _Radius("Radius", Float) = 1.0
        _Center("Center", Vector) = (0.5, 0.5, 0, 0)
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float _Radius;
            float4 _Center;

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv - _Center.xy;
                uv.x *= _ScreenParams.x / _ScreenParams.y;
                float dist = length(uv) * 0.7;
                float alpha = dist > _Radius ? 1.0 : 0.0;
                return fixed4(0, 0, 0, alpha);
            }
            ENDCG
        }
    }
}