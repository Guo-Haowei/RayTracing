ADD_EXECUTABLE(RayTracingInOneWeekend
    Camera.cs
    HittableList.cs
    Hittable.cs
    Main.cs
    Material.cs
    Ray.cs
    Sphere.cs
    Utility.cs
)

SET_PROPERTY(TARGET RayTracingInOneWeekend PROPERTY
    VS_DOTNET_REFERENCES
    "System.Drawing"
    "System.Runtime.Extensions"
    "System.Numerics"
    "System.Numerics.Vectors"
)
