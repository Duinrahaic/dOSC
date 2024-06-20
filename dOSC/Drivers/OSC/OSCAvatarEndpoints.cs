using dOSC.Attributes;
using dOSC.Client.Models.Commands;
using Microsoft.Extensions.Hosting;

namespace dOSC.Drivers.OSC;

public partial class OscService : IHostedService
{
    [ConfigNumericEndpoint(Owner = "VRChat - Avatar",Name = "/avatar/parameters/VRMode", Alias = "VR Mode", Description = "VR Mode", Permissions = Permissions.ReadOnly,
        DefaultValue = 0, Precision = 0)]
    public int VRMode { get; set; } = 0;
    [ConfigNumericEndpoint(Owner = "VRChat - Avatar",Name = "/avatar/parameters/TrackingType", Alias = "Tracking Type", Description = "Tracking Type", Permissions = Permissions.ReadOnly,
        DefaultValue = 0, Precision = 0)]
    public int TrackingType { get; set; } = 0;
    [ConfigNumericEndpoint(Owner = "VRChat - Avatar",Name = "/avatar/parameters/Upright", Alias = "Upright", Description = "Upright", Permissions = Permissions.ReadOnly,
        DefaultValue = 0, MaxValue = 1, MinValue = 0, Precision = 5)]
    public decimal Upright { get; set; } = 0m;
    [ConfigNumericEndpoint(Owner = "VRChat - Avatar", Name = "/avatar/parameters/AngularY", Alias = "AngularY", Description = "Upright", Permissions = Permissions.ReadOnly,
        DefaultValue = 0, MaxValue = 1, MinValue = 0, Precision = 0)]
    public decimal AngularY { get; set; } = 0m;
    [ConfigNumericEndpoint(Owner = "VRChat - Avatar", Name = "/avatar/parameters/GestureLeftWeight", Alias = "Gesture Left Weight", Description = "Gesture Weight on Left Hand", Permissions = Permissions.ReadOnly,
        DefaultValue = 0, MaxValue = 1, MinValue = 0, Precision = 5)]
    public decimal GestureLeftWeight { get; set; } = 0m;
    [ConfigNumericEndpoint(Owner = "VRChat - Avatar",Name = "/avatar/parameters/GestureRightWeight", Alias = "Gesture Right Weight", Description = "Gesture Weight on Right Hand", Permissions = Permissions.ReadOnly,
        DefaultValue = 0, MaxValue = 1, MinValue = 0, Precision = 5)]
    public decimal GestureRightWeight { get; set; } = 0m;
    [ConfigNumericEndpoint(Owner = "VRChat - Avatar",Name = "/avatar/parameters/GestureRight", Alias = "Gesture Right", Description = "Gesture on Right Hand", Permissions = Permissions.ReadOnly,
        DefaultValue = 0, MaxValue = 1, MinValue = 255, Precision = 0)]
    public int GestureRight { get; set; } = 0;
    [ConfigNumericEndpoint(Owner = "VRChat - Avatar",Name = "/avatar/parameters/GestureLeft", Alias = "Gesture Left", Description = "Gesture on Left Hand", Permissions = Permissions.ReadOnly,
        DefaultValue = 0, MaxValue = 1, MinValue = 255, Precision = 0)]
    public int GestureLeft { get; set; } = 0;
    [ConfigNumericEndpoint(Owner = "VRChat - Avatar",Name = "/avatar/parameters/Face", Alias = "Face", Description = "Face", Permissions = Permissions.ReadWrite,
         DefaultValue = 0, MaxValue = 1, MinValue = 0, Precision = 1)]
    public int Face { get; set; } = 0;
    [ConfigNumericEndpoint(Owner = "VRChat - Avatar",Name = "/avatar/parameters/Viseme", Alias = "Viseme", Description = "Viseme", Permissions = Permissions.ReadOnly,
        DefaultValue = 0, MaxValue = 255, MinValue = 0, Precision = 1)]
    public int Viseme { get; set; } = 0;
    [ConfigNumericEndpoint(Owner = "VRChat - Avatar",Name = "/avatar/parameters/VelocityZ", Alias = "VelocityZ", Description = "Velocity in the z-direction", Permissions = Permissions.ReadOnly,
        DefaultValue = 0, MaxValue = 1, MinValue = 0, Precision = 5)]
    public decimal VelocityZ { get; set; } = 0;
    [ConfigNumericEndpoint(Owner = "VRChat - Avatar",Name = "/avatar/parameters/VelocityY", Alias = "VelocityY", Description = "Velocity in the y-direction", Permissions = Permissions.ReadOnly,
        DefaultValue = 0, MaxValue = 1, MinValue = 0, Precision = 5)]
    public decimal VelocityY { get; set; } = 0;
    [ConfigNumericEndpoint(Owner = "VRChat - Avatar",Name = "/avatar/parameters/VelocityX", Alias = "VelocityX", Description = "Velocity in the x-direction", Permissions = Permissions.ReadOnly,
        DefaultValue = 0, MaxValue = 1, MinValue = 0, Precision = 5)]
    public decimal VelocityX { get; set; } = 0;
    [ConfigLogicEndpoint(Owner = "VRChat - Avatar",Name = "/avatar/parameters/InStation", Alias = "In Station", Description = "Enabled when the user is in a station", Permissions = Permissions.ReadOnly,
        DefaultValue = false, TrueLabel = "In Station", FalseLabel = "Not In Station")]
    public bool InStation { get; set; } = false;
    [ConfigLogicEndpoint(Owner = "VRChat - Avatar",Name = "/avatar/parameters/Seated", Alias = "Seated", Description = "Detects when the user is seated", Permissions = Permissions.ReadOnly,
        DefaultValue = false, TrueLabel = "Seated", FalseLabel = "Not Seated")]
    public bool Seated { get; set; } = false;
    [ConfigLogicEndpoint(Owner = "VRChat - Avatar",Name = "/avatar/parameters/AFK", Alias = "Is AFK", Description = "Detects when the user is AFK", Permissions = Permissions.ReadOnly,
        DefaultValue = false, TrueLabel = "AFK", FalseLabel = "Not AFK")]
    public bool AFK { get; set; } = false;
    [ConfigLogicEndpoint(Owner = "VRChat - Avatar",Name = "/avatar/parameters/MuteSelf", Alias = "Is Muted", Description = "Detects when the user is muted", Permissions = Permissions.ReadOnly,
        DefaultValue = false, TrueLabel = "Muted", FalseLabel = "Not Muted")]
    public bool MuteSelf { get; set; } = false;
}