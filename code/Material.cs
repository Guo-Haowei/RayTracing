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

}
