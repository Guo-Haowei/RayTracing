ADD_EXECUTABLE(RayTracingInOneWeekend
    Box.cs
    Camera.cs
    # Dielectric.cs
    DiffuseLight.cs
    HittableList.cs
    Hittable.cs
    Lambertian.cs
    Main.cs
    Material.cs
    # Metal.cs
    OrthonormalBasis.cs
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
