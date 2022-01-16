#ifndef CARTESIAN_INCLUDE
#define CARTESIAN_INCLUDE

//Code to reverse the effects of Unity's Polar Coordinate node: 
// https://github.com/Unity-Technologies/ScriptableRenderPipeline/blob/master/com.unity.shadergraph/Documentation~/Polar-Coordinates-Node.md
// more info at gamedevbill.com
void CartesianCoords_float(float2 PolarCoords, float2 Center, float RadialScale, float LengthScale, out float2 UV)
{
	// reverse the magic number division that occurs inside Unity's node
	float2 adjustedCoord = PolarCoords * float2(0.5, 6.28);
	
	// reverse the scaling factors (why is one called "LengthScale"?  just copying Unity's name, not sure why they called it that)
	adjustedCoord = adjustedCoord / float2(RadialScale, LengthScale);
	
	// standard polar to cartesian math
	float2 result;
	result.x = sin(adjustedCoord.y) * adjustedCoord.x;
	result.y = cos(adjustedCoord.y) * adjustedCoord.x;
	
	//our polar coords had set 0,0 to be at "Center", and we need it to be at the corner instead. 
	UV = result + Center;
}

#endif //CARTESIAN_INCLUDE