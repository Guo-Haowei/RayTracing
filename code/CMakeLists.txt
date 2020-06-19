ADD_EXECUTABLE(RayTracingInOneWeekend
    Camera.cs
    Dielectric.cs
    DiffuseLight.cs
    HittableList.cs
    Hittable.cs
    Lambertian.cs
    Main.cs
    Material.cs
    Metal.cs
    Ray.cs
    Rect.cs
    Sphere.cs
    Texture.cs
    Utility.cs
)

SET_PROPERTY(TARGET RayTracingInOneWeekend PROPERTY
    VS_DOTNET_REFERENCES
    "System.Drawing"
    "System.Runtime.Extensions"
    "System.Numerics"
    "System.Numerics.Vectors"
)
