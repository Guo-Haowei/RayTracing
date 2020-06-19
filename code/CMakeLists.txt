ADD_EXECUTABLE(RayTracingInOneWeekend
    Ray.cs
    Main.cs
)

SET_PROPERTY(TARGET RayTracingInOneWeekend PROPERTY
    VS_DOTNET_REFERENCES
    "System.Drawing"
    "System.Runtime.Extensions"
    "System.Numerics"
    "System.Numerics.Vectors"
)
