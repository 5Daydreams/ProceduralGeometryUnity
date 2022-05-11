using UnityEngine;

namespace _Code.Extensions
{
    public static class AnimationCurveExtension
    {
        private const float DERIVATIVE_DELTA = 0.001f;

        public static float GetDerivative(this AnimationCurve curve, float evalPosition,
            float epsilon = DERIVATIVE_DELTA)
        {
            float positiveOffset = curve.Evaluate(evalPosition + epsilon);
            float negativeOffset = curve.Evaluate(evalPosition - epsilon);

            float tangentVal = (positiveOffset - negativeOffset) * (1.0f / (2.0f * epsilon));

            return tangentVal;
        }

        // public static AnimationCurve operator *(this AnimationCurve curve, float number)
        // {
        //     for (var index = 0; index < curve.keys.Length; index++)
        //     {
        //         Keyframe keyframe = curve.keys[index];
        //         keyframe.value *= number;
        //     }
        //
        //     return curve;
        // }
        
        public static AnimationCurve MultiplyCurve(this AnimationCurve curve, float number)
        {
            Keyframe[] newKeyFrames = new Keyframe[curve.keys.Length];
            
            for (var i = 0; i < newKeyFrames.Length; i++)
            {
                Keyframe newKey = new Keyframe(curve.keys[i].time, curve.keys[i].value * number);
                newKeyFrames[i] = newKey;
            }

            AnimationCurve newCurve = new AnimationCurve(newKeyFrames);
            
            return newCurve;
        }
    }
}