﻿Shader "Grid Shader" {

    Properties{
        _MainTex("Texture Image", 2D) = "white" {}
        _GridThickness("Grid Thickness", Float) = 0.02
        _GridSpacingX("Grid Spacing X", Float) = 10.0
        _GridSpacingY("Grid Spacing Y", Float) = 10.0
        _GridColor("Grid Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_RoadColor("Road Color", Color) = (0.33, 0.52, 0.55, 0.55)
    }

    SubShader{
        Tags {
            "Queue" = "Transparent"
        }

        Pass {

            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            uniform sampler2D _MainTex;
            uniform float _GridThickness;
            uniform float _GridSpacingX;
            uniform float _GridSpacingY;
            uniform float4 _GridColor;

			uniform float4 _RoadColor;
			uniform float2 _RoadPoints[1000];
			uniform int _NumberOfColumns;
			uniform float2 _LeftCoordinates;

            struct vertexInput {
                float4 vertex : POSITION;
                float4 texcoord: TEXCOORD0;
            };

            struct vertexOutput {
                float4 pos : SV_POSITION;
                float4 tex : TEXCOORD0;
                float4 worldPos : TEXCOORD1;
            };

            vertexOutput vert(vertexInput input) {
                vertexOutput output;

                output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
                output.tex = input.texcoord;
                output.worldPos = mul(_Object2World, input.vertex);

                return output;
            }
            
            float4 frag(vertexOutput input) : COLOR {
                if (frac(input.worldPos.x / _GridSpacingX) < _GridThickness ||
                    frac(input.worldPos.y / _GridSpacingY) < _GridThickness) {
                    return _GridColor;
                }
				int row = abs(_LeftCoordinates.y - input.worldPos.y) / _GridSpacingY; 
				int column = abs(input.worldPos.x - _LeftCoordinates.x) / _GridSpacingX;
				if (_RoadPoints[row*_NumberOfColumns + column].x == 1) {
					return _RoadColor;
				}
                return tex2D(_MainTex, input.tex.xy);
            }

            ENDCG
        }
    }
}