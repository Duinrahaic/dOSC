using System;
using dOSC.Attributes;
using dOSC.Client.Models.Commands;

namespace dOSC.Drivers.OSC;

public partial class OscService
{
    public const string ChatboxInput = "/chatbox/input"; // test immedietly? sfx?
    
    
    
    [ConfigLogicEndpoint(Owner = "VRChat-Chat",Name = "/chatbox/typing", Alias = "Enable Typing", Description = "Sets if the chatbox should show typing indicators", Permissions = Permissions.ReadOnly,
        DefaultValue = false, TrueLabel = "Enabled", FalseLabel = "Disabled")]
    public bool ChatboxTyping { get; set; } = false; 

    public void SendChatMessage(string message, bool immediately = false, bool soundEffect = false)
    {
        SendMessage(message, Convert.ToInt32(immediately), Convert.ToInt32(soundEffect));
    }
}