namespace dOSC.Client.Services.Connectors.Hub.OSC
{
    public partial class OSCService
    {
        public const string ChatboxInput = "/chatbox/input"; // test immedietly? sfx?
        public const string ChatboxTyping = "/chatbox/typing"; // bool 

        public void SendChatMessage(string message, bool Immediately = false , bool SoundEffect = false)
        {
            SendMessage(message, Convert.ToInt32(Immediately), Convert.ToInt32(SoundEffect));
        }
    }
}
