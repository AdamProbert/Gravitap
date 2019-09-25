Shader "Minimal Color" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
    }
    SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	    Blend SrcAlpha OneMinusSrcAlpha
        Pass { Color [_Color] }
    }
}