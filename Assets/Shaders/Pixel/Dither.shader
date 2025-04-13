Shader "Hidden/Custom/Dither"
{
    HLSLINCLUDE

    #pragma target 4.5
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

    TEXTURE2D(_MainTex);
    SAMPLER(s_linear_clamp_sampler);
    float4 _MainTex_TexelSize;
    float4 _ScreenSize;

    float _Spread;
    int _RedColorCount, _GreenColorCount, _BlueColorCount;
    int _BayerLevel;

    static const int bayer2[4] = { 0, 2, 3, 1 };
    static const int bayer4[16] = {
        0, 8, 2, 10,
        12, 4, 14, 6,
        3, 11, 1, 9,
        15, 7, 13, 5
    };
    static const int bayer8[64] = {
        0, 32, 8, 40, 2, 34, 10, 42,
        48, 16, 56, 24, 50, 18, 58, 26,
        12, 44, 4, 36, 14, 46, 6, 38,
        60, 28, 52, 20, 62, 30, 54, 22,
        3, 35, 11, 43, 1, 33, 9, 41,
        51, 19, 59, 27, 49, 17, 57, 25,
        15, 47, 7, 39, 13, 45, 5, 37,
        63, 31, 55, 23, 61, 29, 53, 21
    };

    struct Attributes { uint vertexID : SV_VertexID; };
    struct Varyings { float4 positionCS : SV_POSITION; float2 texCoord : TEXCOORD0; };

    Varyings Vert(Attributes input)
    {
        Varyings o;
        float2 pos[3] = { float2(-1, -1), float2(-1, 3), float2(3, -1) };
        float2 uv[3] = { float2(0, 0), float2(0, 2), float2(2, 0) };

        o.positionCS = float4(pos[input.vertexID], 0, 1);
        o.texCoord = uv[input.vertexID];
        return o;
    }

    float GetBayer(int x, int y, int level)
    {
        if (level == 0) return bayer2[(x % 2) + (y % 2) * 2] / 4.0 - 0.5;
        if (level == 1) return bayer4[(x % 4) + (y % 4) * 4] / 16.0 - 0.5;
        return bayer8[(x % 8) + (y % 8) * 8] / 64.0 - 0.5;
    }

    float4 Frag(Varyings i) : SV_Target
    {
        float2 uv = i.texCoord;
        float4 col = SAMPLE_TEXTURE2D(_MainTex, s_linear_clamp_sampler, uv);

        int2 pixelCoords = int2(uv * _ScreenSize.xy);
        float bayer = GetBayer(pixelCoords.x, pixelCoords.y, _BayerLevel);

        
        float luminance = dot(col.rgb, float3(0.299, 0.587, 0.114));
        float curvedSpread = pow(_Spread, 0.75);   
        float strength = curvedSpread * (1.0 - luminance) * 0.25;

        float3 dithered = col.rgb + bayer * strength;
        dithered = saturate(dithered); // prevent over/underflow before quantizing

        dithered.r = floor((_RedColorCount - 1.0) * dithered.r + 0.5) / (_RedColorCount - 1.0);
        dithered.g = floor((_GreenColorCount - 1.0) * dithered.g + 0.5) / (_GreenColorCount - 1.0);
        dithered.b = floor((_BlueColorCount - 1.0) * dithered.b + 0.5) / (_BlueColorCount - 1.0);

        col.rgb = dithered;


        
        col.rgb = saturate(col.rgb);
        return col;
    }

    ENDHLSL

    SubShader
    {
        Tags { "RenderPipeline" = "HDRenderPipeline" }
        Pass
        {
            Name "Dither"
            ZWrite Off
            ZTest Always
            Cull Off
            Blend Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            ENDHLSL
        }
    }
    FallBack Off
}