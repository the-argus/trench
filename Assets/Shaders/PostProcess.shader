Shader "Hidden/PostProcess"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SensitivityDepth("SensitivityDepth", Range(0,5)) = 3.75
        _SensitivityNormals("SensitivityNormals", Range(0,5)) = 0.82
        _SampleDistance("SampleDistance", Range(0,5)) = 1
        _outlineColor("Outline Color", Color) = (0, 0, 0, 0)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            float _SensitivityDepth;
            float _SensitivityNormals;
            float _SampleDistance;
            float4 _outlineColor;

            sampler2D _MainTex;
            sampler2D _CameraDepthNormalsTexture;

            uniform float4 _MainTex_TexelSize;
            uniform float4 _CameraDepthNormalsTexture_TexelSize;

            inline half CheckSame(half2 centerNormal, float centerDepth, half4 theSample)
            {
                // difference in normals
                // do not bother decoding normals - there's no need here
                half2 diff = abs(centerNormal - theSample.xy) * _SensitivityNormals;
                int isSameNormal = (diff.x + diff.y) * _SensitivityNormals < 0.1;
                // difference in depth
                float sampleDepth = DecodeFloatRG(theSample.zw);
                float zdiff = abs(centerDepth - sampleDepth);
                // scale the required threshold by the distance
                int isSameDepth = zdiff * _SensitivityDepth < 0.09 * centerDepth;

                // return:
                // 1 - if normals and depth are similar enough
                // 0 - otherwise

                return isSameNormal * isSameDepth ? 1.0 : 0.0;
            }

            float4 frag(v2f i) : SV_Target
            {
                float2 screenUV = i.uv;
                float sampleSizeX = _CameraDepthNormalsTexture_TexelSize.x;
                float sampleSizeY = _CameraDepthNormalsTexture_TexelSize.y;
                float2 _uv2 = screenUV + float2(-sampleSizeX, -sampleSizeY) * _SampleDistance;
                float2 _uv3 = screenUV + float2(+sampleSizeX, -sampleSizeY) * _SampleDistance;

                //actually sample the UVs
                half4 center = tex2D(_CameraDepthNormalsTexture, screenUV);
                half4 sample1 = tex2D(_CameraDepthNormalsTexture, _uv2);
                half4 sample2 = tex2D(_CameraDepthNormalsTexture, _uv3);

                // encoded normal
                half2 centerNormal = center.xy;
                // decoded depth
                float centerDepth = DecodeFloatRG(center.zw);

                // is it an edge? 0 if yes, 1 if no
                half edge = 1.0;
                edge *= CheckSame(centerNormal, centerDepth, sample1);
                edge *= CheckSame(centerNormal, centerDepth, sample2);

                // calculate this fragment/pixel's color!
                float4 color = tex2D(_MainTex, i.uv);

                color = color * edge + (!edge) * _outlineColor;

                return color;
            }
            ENDCG
        }
    }
}
