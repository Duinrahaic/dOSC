using dOSC.Services.Connectors.OSC;
using dOSC.Services.User;
using dOSC.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.WebSockets;
using System.Security.Policy;

namespace dOSC.Services.Connectors.Activity.Pulsoid
{
    public class PulsoidService : IHostedService, IDisposable
    {
        public delegate void PulsoidSubscriptionEventHandler(PulsoidReading e);
        public event PulsoidSubscriptionEventHandler? OnPulsoidMessageRecieved;
        public bool Running => Setting.IsEnabled;
        private ClientWebSocket? _client;
        private const int ReceiveBufferSize = 256;
        private const string _url = @"wss://dev.pulsoid.net/api/v1/data/real_time";
        private const string _scope = "data:heart_rate:read";
         private Uri _URI => new Uri($"{_url}?access_token={Setting.AccessToken}");
        CancellationTokenSource _CTS = new CancellationTokenSource();
        private ILogger<PulsoidService> _logger;
        private PulsoidSetting? Setting;


        public PulsoidService(IServiceProvider services)
        {
            _logger = services.GetService<ILogger<PulsoidService>>()!;
            _logger.LogInformation("Initialized Pulsoid Service");
            UpdateSettings();
            if(Setting != null )
            {
                if(Setting.IsEnabled)
                {
                    Start();
                }
            }
        }

        public PulsoidSetting? GetSettings()
        {
            return Setting; 
        }

        public void UpdateSettings()
        {
            Setting = (FileSystem.LoadSettings() ?? new()).Pulsoid;
        }
        public void UpdateSetting(PulsoidSetting setting)
        {
            Setting = setting;
            if(Setting != null)
            {
                if (Setting.IsConfigured)
                {
                    if (Setting.IsEnabled)
                    {
                        Stop();
                        Start();
                    }
                }
            }
        }

        public async Task Start()
        {
            if(Setting != null)
            {
               if(Setting.IsConfigured)
                {
                    Task.Run(() => Connect());
                    Setting.IsEnabled = true;
                    FileSystem.SaveSetting(Setting);
                }
            }
        }

        public void Stop()
        {
            if(Setting != null)
            {
                Disconnect();
                Setting.IsEnabled = false;
                FileSystem.SaveSetting(Setting);
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            
            _logger.LogInformation("Pulsoid Service started");
            
        }

        

        private async Task Connect()
        {
            if(_client != null)
                _client.Dispose();
            _client = new();
            await _client.ConnectAsync(_URI, _CTS.Token);
            byte[] buffer = new byte[ReceiveBufferSize];
            if(_client.State == WebSocketState.Open)
            {
                await SendMessage();
            }
            while (_client.State == WebSocketState.Open)
            {
                
                var result = await _client.ReceiveAsync(new ArraySegment<byte>(buffer), _CTS.Token);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _client.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, _CTS.Token);
                    Disconnect();
                }
                else
                {
                    HandleMessage(buffer, result.Count);
                }
            }
            
        }

        private async Task SendMessage()
        {
            if (_client == null) return;
            byte[] bytes = new byte[_scope.Length * sizeof(char)];
            System.Buffer.BlockCopy(_scope.ToCharArray(), 0, bytes, 0, bytes.Length);
            await _client.SendAsync(bytes, WebSocketMessageType.Text, true, _CTS.Token);
        }

        private void HandleMessage(byte[] buffer, int count)
        {
            string json = System.Text.Encoding.Default.GetString(buffer,0,count);
            JObject jobject = JObject.Parse(json);
            PulsoidReading? result = null;
            try
            {
                result = JsonConvert.DeserializeObject<PulsoidReading>(jobject.ToString());
                if(result != null) 
                    OnPulsoidMessageRecieved?.Invoke(result);
            }
            catch
            {

            }
        }

        private void Disconnect()
        {
            if(_client != null)
            {
                _client.Abort();
                _CTS.Cancel();
                _client = null;
                Setting.IsEnabled = false;
                UpdateSetting(Setting);
            }
        }



        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Disconnect();
            _logger.LogInformation("Pulsoid Service stopped");
            await Task.CompletedTask;
        }

 


        public void Dispose() => Disconnect();
    }
}
