namespace dOSC.Drivers.Websocket;

public class WebSocketConfiguration
{
    public bool Enabled { get; set; } = true; 
    public int Port { get; set; } = 5880;
    
    // Requires a valid API key to connect
    public bool Secure { get; set; } = false;
    public string ApiKey { get; set; } = "1234";
    
}