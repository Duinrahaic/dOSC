using dOSC.Attributes;
using dOSC.Client.Models.Commands;

namespace dOSC.Drivers.VRChat;

public partial class VRChatService
{
    
    #region Buttons 
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
    
    #endregion Buttons Inputs
    
    #region Avatar 
    [ConfigTextEndpoint(Owner = "VRChat-Avatar",Name = "/avatar/change", Alias = "Current Avatar Id", Description = "Current Avatar Id", Permissions = Permissions.ReadOnly,
        DefaultValue = "Unknown")]
    public string CurrentAvatarId
    {
        get => _currentAvatarId;
        set
        {
            _currentAvatarId = value;
            CurrentAvatarName = _avatarConfigs.FirstOrDefault(x => x.Id == value)?.Name ?? "Unknown";
        }
    } 
    private string _currentAvatarId = "Unknown";

    [ConfigTextEndpoint(Owner = "VRChat-Avatar",Name = "/avatar/parameters/AvatarName", Alias = "Current Avatar Name", Description = "Current Avatar Name", Permissions = Permissions.ReadOnly,
        DefaultValue = "Unknown")]
    public string CurrentAvatarName { get => _currentAvatarName;
        set
        {
            _currentAvatarName = value;
            var endpoint = HubService.GetEndpointByAddress("VRChat-Avatar", "/avatar/parameters/AvatarName");
            if (endpoint != null)
            {
                var val = endpoint.ToDataEndpointValue();
                val.UpdateValue(value);
                HubService.UpdateEndpointValue(val);
            }
        }
    } 
    private string _currentAvatarName = "Unknown";

    [ConfigNumericEndpoint(Owner = "VRChat-Avatar",Name = "/avatar/parameters/VRMode", Alias = "VR Mode", Description = "VR Mode", Permissions = Permissions.ReadOnly,
        DefaultValue = 0, Precision = 0)]
    public int VRMode { get; set; } = 0;
    [ConfigNumericEndpoint(Owner = "VRChat-Avatar",Name = "/avatar/parameters/TrackingType", Alias = "Tracking Type", Description = "Tracking Type", Permissions = Permissions.ReadOnly,
        DefaultValue = 0, Precision = 0)]
    public int TrackingType { get; set; } = 0;
    [ConfigNumericEndpoint(Owner = "VRChat-Avatar",Name = "/avatar/parameters/Upright", Alias = "Upright", Description = "Upright", Permissions = Permissions.ReadOnly,
        DefaultValue = 0, MaxValue = 1, MinValue = 0, Precision = 5)]
    public decimal Upright { get; set; } = 0m;
    [ConfigNumericEndpoint(Owner = "VRChat-Avatar", Name = "/avatar/parameters/AngularY", Alias = "AngularY", Description = "Upright", Permissions = Permissions.ReadOnly,
        DefaultValue = 0, MaxValue = 1, MinValue = 0, Precision = 0)]
    public decimal AngularY { get; set; } = 0m;
    [ConfigNumericEndpoint(Owner = "VRChat-Avatar", Name = "/avatar/parameters/GestureLeftWeight", Alias = "Gesture Left Weight", Description = "Gesture Weight on Left Hand", Permissions = Permissions.ReadOnly,
        DefaultValue = 0, MaxValue = 1, MinValue = 0, Precision = 5)]
    public decimal GestureLeftWeight { get; set; } = 0m;
    [ConfigNumericEndpoint(Owner = "VRChat-Avatar",Name = "/avatar/parameters/GestureRightWeight", Alias = "Gesture Right Weight", Description = "Gesture Weight on Right Hand", Permissions = Permissions.ReadOnly,
        DefaultValue = 0, MaxValue = 1, MinValue = 0, Precision = 5)]
    public decimal GestureRightWeight { get; set; } = 0m;
    [ConfigNumericEndpoint(Owner = "VRChat-Avatar",Name = "/avatar/parameters/GestureRight", Alias = "Gesture Right", Description = "Gesture on Right Hand", Permissions = Permissions.ReadOnly,
        DefaultValue = 0, MaxValue = 1, MinValue = 255, Precision = 0)]
    public int GestureRight { get; set; } = 0;
    [ConfigNumericEndpoint(Owner = "VRChat-Avatar",Name = "/avatar/parameters/GestureLeft", Alias = "Gesture Left", Description = "Gesture on Left Hand", Permissions = Permissions.ReadOnly,
        DefaultValue = 0, MaxValue = 1, MinValue = 255, Precision = 0)]
    public int GestureLeft { get; set; } = 0;
    [ConfigNumericEndpoint(Owner = "VRChat-Avatar",Name = "/avatar/parameters/Face", Alias = "Face", Description = "Face", Permissions = Permissions.ReadWrite,
         DefaultValue = 0, MaxValue = 1, MinValue = 0, Precision = 1)]
    public int Face { get; set; } = 0;
    [ConfigNumericEndpoint(Owner = "VRChat-Avatar",Name = "/avatar/parameters/Viseme", Alias = "Viseme", Description = "Viseme", Permissions = Permissions.ReadOnly,
        DefaultValue = 0, MaxValue = 255, MinValue = 0, Precision = 1)]
    public int Viseme { get; set; } = 0;
    [ConfigNumericEndpoint(Owner = "VRChat-Avatar",Name = "/avatar/parameters/VelocityZ", Alias = "VelocityZ", Description = "Velocity in the z-direction", Permissions = Permissions.ReadOnly,
        DefaultValue = 0, MaxValue = 1, MinValue = 0, Precision = 5)]
    public decimal VelocityZ { get; set; } = 0;
    [ConfigNumericEndpoint(Owner = "VRChat-Avatar",Name = "/avatar/parameters/VelocityY", Alias = "VelocityY", Description = "Velocity in the y-direction", Permissions = Permissions.ReadOnly,
        DefaultValue = 0, MaxValue = 1, MinValue = 0, Precision = 5)]
    public decimal VelocityY { get; set; } = 0;
    [ConfigNumericEndpoint(Owner = "VRChat-Avatar",Name = "/avatar/parameters/VelocityX", Alias = "VelocityX", Description = "Velocity in the x-direction", Permissions = Permissions.ReadOnly,
        DefaultValue = 0, MaxValue = 1, MinValue = 0, Precision = 5)]
    public decimal VelocityX { get; set; } = 0;
    [ConfigLogicEndpoint(Owner = "VRChat-Avatar",Name = "/avatar/parameters/InStation", Alias = "In Station", Description = "Enabled when the user is in a station", Permissions = Permissions.ReadOnly,
        DefaultValue = false, TrueLabel = "In Station", FalseLabel = "Not In Station")]
    public bool InStation { get; set; } = false;
    [ConfigLogicEndpoint(Owner = "VRChat-Avatar",Name = "/avatar/parameters/Seated", Alias = "Seated", Description = "Detects when the user is seated", Permissions = Permissions.ReadOnly,
        DefaultValue = false, TrueLabel = "Seated", FalseLabel = "Not Seated")]
    public bool Seated { get; set; } = false;
    [ConfigLogicEndpoint(Owner = "VRChat-Avatar",Name = "/avatar/parameters/AFK", Alias = "Is AFK", Description = "Detects when the user is AFK", Permissions = Permissions.ReadOnly,
        DefaultValue = false, TrueLabel = "AFK", FalseLabel = "Not AFK")]
    public bool AFK { get; set; } = false;
    [ConfigLogicEndpoint(Owner = "VRChat-Avatar",Name = "/avatar/parameters/MuteSelf", Alias = "Is Muted", Description = "Detects when the user is muted", Permissions = Permissions.ReadOnly,
        DefaultValue = false, TrueLabel = "Muted", FalseLabel = "Not Muted")]
    public bool MuteSelf { get; set; } = false;
    
    #endregion Avatar
    
    #region Chatbox
    
    public const string ChatboxInput = "/chatbox/input"; // test immedietly? sfx?
    
    [ConfigLogicEndpoint(Owner = "VRChat-Chat",Name = "/chatbox/typing", Alias = "Enable Typing", Description = "Sets if the chatbox should show typing indicators", Permissions = Permissions.ReadOnly,
        DefaultValue = false, TrueLabel = "Enabled", FalseLabel = "Disabled")]
    public bool ChatboxTyping { get; set; } = false; 

    public void SendChatMessage(string message, bool immediately = false, bool soundEffect = false)
    {
        _oscService.SendMessage(message, Convert.ToInt32(immediately), Convert.ToInt32(soundEffect));
    }
    
    #endregion

    #region Analog Inputs
    public const string Vertical = "/input/Vertical";
    public const string Horizontal = "/input/Horizontal";
    public const string LookHorizontal = "/input/LookHorizontal";
    public const string LookVertical = "/input/LookVertical";
    public const string UseAxisRight = "/input/UseAxisRight";
    public const string GrabAxisRight = "/input/GrabAxisRight";
    public const string MoveHoldFB = "/input/MoveHoldFB";
    public const string SpinHoldCwCcw = "/input/SpinHoldCwCcw";
    public const string SpinHoldUD = "/input/SpinHoldUD";
    public const string SpinHoldLR = "/input/SpinHoldLR";
    

    #endregion Analog Inputs
}