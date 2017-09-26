#version 450
#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : enable
layout (input_attachment_index = 0, set = 0, binding = 0) uniform subpassInput vkInputAttachmentGUI;
layout (location = 0) in vec2 texcoord;
layout (location = 0) out vec4 fragColor;

const float TWO_PI = 6.28318530718;
const float distThresh = 0.4;
const float baseRadius = 0.1;
const float brightAdjust = 4.;
const int numControlPoints = 12;

void main() 
{
	
	//fragColor = vec4(0.0,1.0,0,0);//subpassLoad(vkInputAttachmentGUI);
        
       //fragColor = vec4((gl_FragCoord.xy) - floor(gl_FragCoord.xy),0.0,1.0);

    vec2  uv = (2. * gl_FragCoord.xy - vec2(512.0,512.0)) / 512.0,
          center = vec2(0);
    float speed = 1.0,
          time = texcoord.x * speed,
          radius = baseRadius + baseRadius * 0.98 * sin(time), // 0.98 to reduce aliasing when all circles overlap
          dist = 0.,
          segmentRads = TWO_PI / float(numControlPoints);
    
	// create control points in a circle and check distance sum
    for(int i=0; i < numControlPoints; i++) {
        float curRads = segmentRads * float(i);
        float curRadius = radius * 2.;
        vec2 ctrlPoint = vec2(sin(curRads) * curRadius, cos(curRads) * curRadius);
        if(distance(uv, ctrlPoint) < distThresh) dist += distance(uv, ctrlPoint);
    }
    
    // adjust distance to compensate for numControlPoints addition
    dist /= float(numControlPoints);

    vec4 guiColor = subpassLoad(vkInputAttachmentGUI);
    vec4 color = vec4(vec3(dist * brightAdjust), 1.0);

    fragColor = color;//mix(color, guiColor, guiColor.x);
}