using System;
using System.Numerics;

namespace RayTracingInOneWeekend {

public abstract class Material
{
    public abstract bool scatter(in Ray ray, in HitRecord record, ref Vector3 attenuation, ref Ray scattered);
}

public class Lambertian : Material
{
    public Lambertian(in Vector3 albedo)
    {
        this.albedo = albedo;
    }

    public override bool scatter(in Ray ray, in HitRecord record, ref Vector3 attenuation, ref Ray scattered)
    {
        scattered.origin = record.point;
        scattered.direction = record.normal + Utility.RandomUnitVector();
        attenuation = albedo;
        return true;
    }

    private readonly Vector3 albedo;
}

public class Metal : Material
{
    public Metal(in Vector3 albedo, float fuzziness = 0.0f)
    {
        this.albedo = albedo;
        this.fuzziness = fuzziness;
    }

    public override bool scatter(in Ray ray, in HitRecord record, ref Vector3 attenuation, ref Ray scattered)
    {
        Vector3 reflected = Vector3.Reflect(Vector3.Normalize(ray.direction), record.normal);
        scattered.origin = record.point;
        scattered.direction = reflected + fuzziness * Utility.RandomUnitVector();
        attenuation = albedo;
        return Vector3.Dot(scattered.direction, record.normal) > 0.0f;
    }

    private readonly Vector3 albedo;
    private readonly float fuzziness;
}

public class Dielectric : Material
{
    public Dielectric(float reflectionPower)
    {
        this.reflectionPower = reflectionPower;
    }
    public override bool scatter(in Ray ray, in HitRecord record, ref Vector3 attenuation, ref Ray scattered)
    {
        attenuation = Vector3.One;
        float factor = record.frontFace ? 1.0f / reflectionPower : reflectionPower;
        Vector3 unitDirection =  Vector3.Normalize(ray.direction);

        scattered.origin = record.point;

        float cosTheta = Math.Min(-Vector3.Dot(unitDirection, record.normal), 1.0f);
        float sinTheta = Utility.SqrtF(1.0f - cosTheta * cosTheta);
        float reflectionProbability = Utility.Schlick(cosTheta, factor);

        if (factor * sinTheta > 1.0f || Utility.RandomF() < reflectionProbability)
            scattered.direction = Vector3.Reflect(unitDirection, record.normal);
        else
            scattered.direction = Utility.Refract(unitDirection, record.normal, factor);

        return true;
    }

    readonly float reflectionPower;
}

}
