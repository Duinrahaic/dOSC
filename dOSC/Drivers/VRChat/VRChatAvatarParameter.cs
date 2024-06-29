namespace dOSC.Drivers.VRChat;

public class VRChatAvatarParameter
{
    public string Name { get; set; } = string.Empty;
    public VRChatAvatarParameterEndpoint? Output { get; set; }
    public VRChatAvatarParameterEndpoint? Input { get; set; }
}