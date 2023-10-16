namespace dOSC.Services.Connectors.Game.VRChat
{
    public class VRChatLogMonitor : IHostedService, IDisposable
    {

        private string ActiveLogName { get; set; } = string.Empty;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;

        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        public void Dispose()
        {
        }
    }
    /* 
        Invite Responses Types
        type:invite <- blue invite
        type:inviteResponse <- pink response
        
        Invite Notifications
        from AllTime: Received
        from Recent: Cleared/Responded
        
        Id: not_GUID <- Relationship

        From: username:username <- Who sent the invite/response

        Regex for Cleared/Responded:
            (?:type:inviteResponse)|(?:from Recent)|(?:from username:(.*?),)|(?:(not_(.*?),))    
        
     
     */
}
