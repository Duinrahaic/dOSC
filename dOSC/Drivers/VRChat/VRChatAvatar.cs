namespace dOSC.Drivers.VRChat;

public class VRChatAvatar
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<VRChatAvatarParameter> Parameters { get; set; } = new();
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}