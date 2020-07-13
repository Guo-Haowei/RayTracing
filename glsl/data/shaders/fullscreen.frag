#version 460 core
layout (location = 0) out vec4 out_color;
layout (location = 0) in vec2 pass_uv;

uniform sampler2D outImage;

void main()
{
    vec4 color4 = texture(outImage, pass_uv);
    vec3 color = color4.rgb / color4.a;

    // gamma correction
    float gamma = 2.2;
    color = color / (color + 1.0);
    color = pow(color, vec3(1. / gamma));

    out_color = vec4(color, 1.0);
}
