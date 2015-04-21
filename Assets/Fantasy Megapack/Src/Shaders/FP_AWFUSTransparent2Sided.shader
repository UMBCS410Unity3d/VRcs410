Shader "AWFUS/FP/Transparent2Sided" {
Properties {
  _Color ("Main Color", Color) = (1,1,1,1)
  
  _MainTex ("Base (RGB)", 2D) = "white" {}

  _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
  _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
  _Gloss("Gloss", Range (0.00, 1)) = .5
  _SpecMap ("Specular (R) Gloss (G) Reflect (B)", 2D) = "white" {}
  
  _Reflect("Reflect", Range (0.00, 1)) = .5
  _Cube ("Cubemap", CUBE) = "" {}

  _BumpMap ("Normalmap", 2D) = "bump" {}

  _RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
  _RimPower ("Rim Power", Range(0.5,8.0)) = 3.0

  _WrapTex  ("Fresnel ramp (R), Rim ramp (G)", 2D) = "black" {}
}
SubShader { 
  Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
  LOD 400
  Cull Off
  
CGPROGRAM
#pragma surface surf BlinnPhong alpha
#pragma target 3.0


fixed4 _Color;
sampler2D _MainTex;
sampler2D _BumpMap;
sampler2D _SpecMap;
sampler2D _WrapTex;
samplerCUBE _Cube;
float4 _RimColor;
half _RimPower;
half _Shininess;
half _Gloss;
half _Reflect;


struct Input {
  float2 uv_MainTex;
  float3 worldRefl;
  float3 viewDir;
  INTERNAL_DATA
};

void surf (Input IN, inout SurfaceOutput o) {
  
  // Diffuse
  fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
  o.Albedo = tex.rgb * _Color.rgb;
  o.Alpha  = tex.a * _Color.a; 

  // Specular
  fixed4 spec = tex2D(_SpecMap, IN.uv_MainTex);
  o.Specular = spec.r * _Shininess;
  o.Gloss = spec.g * _Gloss;

  // Normal
  float3 normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
  o.Normal = normal;

  // Emission
  half diffusePos = saturate(dot(normal, IN.viewDir));
  half4 wrap = tex2D (_WrapTex, diffusePos.xx);
  half3 rim = _RimColor.rgb * pow(wrap.g, _RimPower);
  o.Emission = texCUBE(_Cube, IN.worldRefl).rgb * _Reflect * spec.b * wrap.r + rim;

}
ENDCG
}

FallBack "Reflective/Bumped Specular"
}
