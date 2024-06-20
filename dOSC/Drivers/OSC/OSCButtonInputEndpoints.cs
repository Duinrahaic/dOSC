using dOSC.Attributes;
using dOSC.Client.Models.Commands;

namespace dOSC.Drivers.OSC;

public partial class OscService
{
  
    [ConfigLogicEndpoint(Owner = "VRChat-Buttons", Name = "/input/MoveForward", Alias = "Move Forward", Description = "Move Forward", 
        Permissions = Permissions.WriteOnly,
        DefaultValue = false, TrueLabel = "Pressed", FalseLabel = "Released")]
    public bool MoveForward { get; set; } = false;
    
    [ConfigLogicEndpoint(Owner = "VRChat-Buttons", Name = "/input/MoveBackward", Alias = "Move Backward", Description = "Move Backward", 
        Permissions = Permissions.WriteOnly,
        DefaultValue = false, TrueLabel = "Pressed", FalseLabel = "Released")]
    public bool MoveBackward { get; set; } = false;
    
    [ConfigLogicEndpoint(Owner = "VRChat-Buttons", Name = "/input/MoveLeft", Alias = "Move Left", Description = "Move Left", 
        Permissions = Permissions.WriteOnly,
        DefaultValue = false, TrueLabel = "Pressed", FalseLabel = "Released")]
    public bool MoveLeft { get; set; } = false;
    
    [ConfigLogicEndpoint(Owner = "VRChat-Buttons", Name = "/input/MoveRight", Alias = "Move Right", Description = "Move Right", 
        Permissions = Permissions.WriteOnly,
        DefaultValue = false, TrueLabel = "Pressed", FalseLabel = "Released")]
    public bool MoveRight { get; set; } = false;
    
    [ConfigLogicEndpoint(Owner = "VRChat-Buttons", Name = "/input/LookRight", Alias = "Look Right", Description = "Look Right", 
        Permissions = Permissions.WriteOnly,
        DefaultValue = false, TrueLabel = "Pressed", FalseLabel = "Released")]
    public bool LookRight { get; set; } = false;
    
    [ConfigLogicEndpoint(Owner = "VRChat-Buttons", Name = "/input/LookDown", Alias = "Look Down", Description = "Look Down", 
        Permissions = Permissions.WriteOnly,
        DefaultValue = false, TrueLabel = "Pressed", FalseLabel = "Released")]
    public bool LookDown { get; set; } = false;
    
    [ConfigLogicEndpoint(Owner = "VRChat-Buttons", Name = "/input/LookUp", Alias = "Look Up", Description = "Look Up", 
        Permissions = Permissions.WriteOnly,
        DefaultValue = false, TrueLabel = "Pressed", FalseLabel = "Released")]
    public bool LookUp { get; set; } = false;
    
    [ConfigLogicEndpoint(Owner = "VRChat-Buttons", Name = "/input/Jump", Alias = "Jump", Description = "Jump if the world supports it.", 
        Permissions = Permissions.WriteOnly,
        DefaultValue = false, TrueLabel = "Pressed", FalseLabel = "Released")]
    public bool Jump { get; set; } = false;
    
    [ConfigLogicEndpoint(Owner = "VRChat-Buttons", Name = "/input/Run", Alias = "Run", Description = "Walk faster if the world supports it.", 
        Permissions = Permissions.WriteOnly,
        DefaultValue = false, TrueLabel = "Pressed", FalseLabel = "Released")]
    public bool MovementRun { get; set; } = false;
    
    [ConfigLogicEndpoint(Owner = "VRChat-Buttons", Name = "/input/ComfortLeft", Alias = "Snap-Turn Left", Description = " Snap-Turn to the Left - VR Only.",
        Permissions = Permissions.WriteOnly,
        DefaultValue = false, TrueLabel = "Pressed", FalseLabel = "Released")]
    public bool ComfortLeft { get; set; } = false;
    
    [ConfigLogicEndpoint(Owner = "VRChat-Buttons", Name = "/input/ComfortRight", Alias = "Snap-Turn Right", Description = " Snap-Turn to the right - VR Only.",
        Permissions = Permissions.WriteOnly,
        DefaultValue = false, TrueLabel = "Pressed", FalseLabel = "Released")]
    public bool ComfortRight { get; set; } = false;
    
    [ConfigLogicEndpoint(Owner = "VRChat-Buttons", Name = "/input/DropRight", Alias = "Drop Right", Description = "Drop the item held in your Right hand - VR Only.",
        Permissions = Permissions.WriteOnly,
        DefaultValue = false, TrueLabel = "Pressed", FalseLabel = "Released")]
    public bool DropRight { get; set; } = false;
    
    [ConfigLogicEndpoint(Owner = "VRChat-Buttons", Name = "/input/UseRight", Alias = "Use Right", Description = "Use the item highlighted by your Right hand - VR Only.",
        Permissions = Permissions.WriteOnly,
        DefaultValue = false, TrueLabel = "Pressed", FalseLabel = "Released")]
    public bool UseRight { get; set; } = false;
    
    [ConfigLogicEndpoint(Owner = "VRChat-Buttons", Name = "/input/GrabRight", Alias = "Grab Right", Description = "Grab the item highlighted by your Right hand - VR Only.",
        Permissions = Permissions.WriteOnly,
        DefaultValue = false, TrueLabel = "Pressed", FalseLabel = "Released")]
    public bool GrabRight { get; set; } = false;
    
    [ConfigLogicEndpoint(Owner = "VRChat-Buttons", Name = "/input/DropLeft", Alias = "Drop Left", Description = "Drop the item held in your left hand - VR Only.",
        Permissions = Permissions.WriteOnly,
        DefaultValue = false, TrueLabel = "Pressed", FalseLabel = "Released")]
    public bool DropLeft { get; set; } = false;
    
    [ConfigLogicEndpoint(Owner = "VRChat-Buttons", Name = "/input/UseLeft", Alias = "Use Left", Description = "Use the item highlighted by your left hand - VR Only.",
        Permissions = Permissions.WriteOnly,
        DefaultValue = false, TrueLabel = "Pressed", FalseLabel = "Released")]
    public bool UseLeft { get; set; } = false;
    
    [ConfigLogicEndpoint(Owner = "VRChat-Buttons", Name = "/input/GrabLeft", Alias = "Grab Left", Description = "Grab the item highlighted by your left hand - VR Only.",
        Permissions = Permissions.WriteOnly,
        DefaultValue = false, TrueLabel = "Pressed", FalseLabel = "Released")]
    public bool GrabLeft { get; set; } = false;
  
    [ConfigLogicEndpoint(Owner = "VRChat-Buttons", Name = "/input/PanicButton", Alias = "Panic Mode", Description = "Enables or disables panic mode for the user",
        Permissions = Permissions.WriteOnly,
        DefaultValue = false, TrueLabel = "Pressed", FalseLabel = "Released")]
    public bool PanicButton { get; set; } = false;
    
    [ConfigLogicEndpoint(Owner = "VRChat-Buttons", Name = "/input/Menu", Alias = "Toggle Menu", Description = "Toggles the menu on or off",
        Permissions = Permissions.WriteOnly,
        DefaultValue = false, TrueLabel = "Pressed", FalseLabel = "Released")]
    public bool Menu { get; set; } = false;
    
    [ConfigLogicEndpoint(Owner = "VRChat-Buttons", Name = "/input/QuickMenuToggleLeft", Alias = "Toggle Left Quick Menu ", Description = "Toggles the quick menu on the left hand",
        Permissions = Permissions.WriteOnly,
        DefaultValue = false, TrueLabel = "Pressed", FalseLabel = "Released")]
    public bool QuickMenuToggleLeft { get; set; } = false;
    
    [ConfigLogicEndpoint(Owner = "VRChat-Buttons", Name = "/input/QuickMenuToggleRight", Alias = "Toggle Right Quick Menu", Description = "Toggles the quick menu on the right hand",
        Permissions = Permissions.WriteOnly,
        DefaultValue = false, TrueLabel = "Pressed", FalseLabel = "Released")]
    public bool QuickMenuToggleRight { get; set; } = false;
    
    [ConfigLogicEndpoint(Owner = "VRChat-Buttons", Name = "/input/ToggleSitStand", Alias = "Crouch", Description = "Toggles users crouch on or off",
        Permissions = Permissions.WriteOnly,
        DefaultValue = false, TrueLabel = "Pressed", FalseLabel = "Released")]
    public bool ToggleSitStand { get; set; } = false;
    
    [ConfigLogicEndpoint(Owner = "VRChat-Buttons", Name = "/input/AFKToggle", Alias = "AFK Toggle", Description = "Toggles users AFK Mode on or off",
        Permissions = Permissions.WriteOnly,
        DefaultValue = false, TrueLabel = "Pressed", FalseLabel = "Released")]
    public bool AFKToggle { get; set; } = false;

    [ConfigLogicEndpoint(Owner = "VRChat-Buttons", Name = "/input/Voice", Alias = "Voice", Description = "Toggles users voice on or off",
        Permissions = Permissions.WriteOnly,
        DefaultValue = false, TrueLabel = "Pressed", FalseLabel = "Released")]
    public bool Voice { get; set; } = false;
}