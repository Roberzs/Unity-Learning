// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/My First Shader"
{
    // 属性块
    Properties {
        _Tint ("Tint", COLOR) = (1, 1, 1, 1)
        _MainTex("MainTex", 2D) = "white"{}
    }

    // 每个shader至少有一个子着色器
    SubShader {
        // 每个着色器至少有一个通道
        pass {
            // 要用CGPROGRAM、ENDCG指示代码的开始与结束
            CGPROGRAM 
            // 指定顶点与片元程序
            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram

            // 引用其他文件
            // UnityCG.cginc: 包含了最常使用的帮助函数、 宏和结构体
            // UnityShaderVariables.cginc: 定义了渲染所需的一堆着色器变量，例如变换，相机和光照数据
            // HLSLSupport.cginc: 内部声明了很多用于跨平台编译的宏和定义
            // Lighting.cginc: 包含了各种内置的光照模型，对于表面着色器会自动包含进来
            #include "UnityCG.cginc"

            // 属性定义
            float4 _Tint;
            sampler2D _MainTex;
            float4 _MainTex_ST;     // _ST后缀代表缩放与平移

            // POSITION语义:用模型空间顶点坐标填充position
            // TEXCOORD0: 用模型第一套纹理坐标填充localPosition
            struct Interpolators {
                float4 position : SV_POSITION;
            //    float3 localPosition: TEXCOORD0;
                float2 uv : TEXCOORD0;
            };

            struct VertexData {
                float4 position : POSITION;
                float2 uv: TEXCOORD0;
            
            };

            // 顶点程序用于计算返回顶点的最终坐标(float4型) 并且需要告诉编译器返回的数据代表着什么，SV代表系统值，POSITION代表最终顶点位置 (↑)
            Interpolators MyVertexProgram(VertexData v) {
                Interpolators i;
                //i.localPosition = v.position.xyz;
                // 传入的坐标为对象的空间坐标 显示时要将模型空间坐标转为裁剪空间坐标 UNITY_MATRIX_MVP
                i.position = UnityObjectToClipPos(v.position);
                i.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                return i; 
            }

            // 片元程序用于计算每个像素输出的RGBA颜色值(float4型) 需要使用的语义SV_TARGET 且顶点程序的输出为片元程序的输入，同样输入参数需标识SV_POSITION
            float4 MyFragmentProgram(Interpolators i) : SV_TARGET{
                // +0.5f是因为坐标区间位于 (-1/2)-(1/2) 之间 
                //return float4(i.localPosition + 0.5f, 1) * _Tint;
                
                return tex2D(_MainTex, i.uv) * _Tint;
            }

            ENDCG
        }
    }
}
