#ifndef MYTEST_HLSL
#define MYTEST_HLSL

void Test_float(float A, float B, float3 AB, float3 BA, float3 AA, out float3 Out)
{
    if(A<B)
    {
        Out = AB;
    }
    else {
        if (B < A) {
            Out = BA;
        }
        else {
            Out = AA;
        }
    }
 

}
#endif