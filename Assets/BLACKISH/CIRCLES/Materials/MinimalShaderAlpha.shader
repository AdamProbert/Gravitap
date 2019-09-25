Shader "Minimal Alpha" {
    Properties {
        _Color ("Main Color", Color) = (1.0,1.0,1.0,0.5)
        _MainTex ("Texture", 2D) = "white" { }
    }

    SubShader {
 		Cull Off
		Tags { "Queue" = "Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
       Pass {
            Material {
                Diffuse [_Color]
                Ambient [_Color]
            }
            Lighting Off
            Color (0.5, 0.5, 0.5)
            SetTexture [_MainTex] {
                constantColor [_Color]
                Combine texture * constant
            }
        }
    }
} 