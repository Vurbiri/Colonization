using UnityEngine;
using static UnityEngine.ParticleSystem;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	public static class ParticleSystemExtensions
	{
        [Impl(256)] public static float GetAvgSpeed(this in MinMaxCurve self)
        {
            return self.mode switch
            {
                ParticleSystemCurveMode.Constant => self.constant,
                ParticleSystemCurveMode.TwoConstants => (self.constantMin + self.constantMax) * 0.5f,
                _ => self.curveMultiplier
            };
        }
    }
}
