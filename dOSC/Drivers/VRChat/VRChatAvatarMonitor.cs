using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace dOSC.Drivers.VRChat;

public partial class VRChatService
{
    public delegate void AvatarMemoryCountChanged(int count);
    public event AvatarMemoryCountChanged? OnAvatarMemoryCountChanged;
    public delegate void AvatarScanComplete();
    public event AvatarScanComplete? OnAvatarScanComplete;
    
    private List<VRChatAvatar> _learnedAvatarConfigs = new();
    public List<VRChatAvatar> GetLearnedAvatarConfigs()
    {
        return _learnedAvatarConfigs;
    }
    
    public void LearnAvatarConfig(VRChatAvatar avatar)
    {
        if (!_learnedAvatarConfigs.Contains(avatar))
        {
            _learnedAvatarConfigs.Add(avatar);
            LearnedAvatarConfigCount = _learnedAvatarConfigs.Count;
        }
    }
    
    public void ForgetAvatarConfig(VRChatAvatar avatar)
    {
        if (_learnedAvatarConfigs.Contains(avatar))
        {
            _learnedAvatarConfigs.Remove(avatar);
            LearnedAvatarConfigCount = _learnedAvatarConfigs.Count;
        }

    }
    
    public void ForgetAllAvatarConfigs()
    {
        _learnedAvatarConfigs = new List<VRChatAvatar>();
        LearnedAvatarConfigCount = _learnedAvatarConfigs.Count;

    }
    
    public bool IsAvatarConfigLearned(string id)
    {
        return _learnedAvatarConfigs.Any(avatar => avatar.Id == id);
        LearnedAvatarConfigCount = _learnedAvatarConfigs.Count;

    }

    public void RelearnAvatarConfig(VRChatAvatar avatar)
    {
        if (_learnedAvatarConfigs.Any(x=> x.Id ==  avatar.Id))
        {
            _learnedAvatarConfigs.Remove(_learnedAvatarConfigs.First(x=> x.Id ==  avatar.Id));
        }
        LearnAvatarConfig(avatar);
    }
    
    private List<VRChatAvatar> _avatarConfigs = new();
    public int LearnedAvatarConfigCount
    {
        get => _learnedAvatarConfigCount;
        private set
        {
            if (_learnedAvatarConfigCount != value)
            {
                _learnedAvatarConfigCount = value;
                OnAvatarMemoryCountChanged?.Invoke(value);
            }
        }
    } 
    private int _learnedAvatarConfigCount = 0;
    public List<VRChatAvatar> GetAvatarConfigs()
    {
        return _avatarConfigs;
    }
    
    private FileSystemWatcher _watcher;
    private bool _allowAvatarConfigLearning = true;
    public bool AllowAvatarConfigLearning
    {
        get => _allowAvatarConfigLearning;
        set
        {
            if (_allowAvatarConfigLearning != value)
            {
                _allowAvatarConfigLearning = value;
                var config = GetConfiguration();
                config.AllowAvatarConfigLearning = value;
                SaveConfiguration(config);
                if (_allowAvatarConfigLearning)
                {
                    Watch();
                }
                else
                {
                    Unwatch();
                }
            }
        }
    }
    private string DefaultAvatarConfigPath =>  Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).Replace("Local", "LocalLow"), "VRChat", "VRChat", "OSC");
    
    
    private void Watch()
    {
        
        _watcher = new FileSystemWatcher
        {
            Path = DefaultAvatarConfigPath,
            NotifyFilter = NotifyFilters.LastWrite ,
            Filter = "*.json",
            IncludeSubdirectories = true
        };
        
        _watcher.Changed += OnChanged;
        _watcher.Created += OnChanged;
        _watcher.Deleted += OnChanged;
    }
    
    private void Unwatch()
    {
        if(_watcher == null) return;
        _watcher.Changed -= OnChanged;
        _watcher.Created -= OnChanged;
        _watcher.Deleted -= OnChanged;
    }

    private void OnChanged(object source, FileSystemEventArgs e)
    {
        var avatar = ReadFile(e.FullPath);
        if (avatar != null && IsAvatarConfig(avatar.Id))
        {
            _avatarConfigs.Add(avatar);
        }
        
    }

  
    private void ScanAndProcessFiles(string path)
    {
        string absoluteFilePath = Path.GetFullPath(path);
        foreach (string subDir in Directory.GetDirectories(absoluteFilePath))
        {
            foreach (string file in Directory.GetFiles(subDir))
            {
                var avatar = ReadFile(file);
                if (avatar != null && IsAvatarConfig(avatar.Id))
                {
                    _avatarConfigs.Add(avatar);
                }
            }

            // Recursively scan subdirectories
            ScanAndProcessFiles(subDir);
        }
        OnAvatarScanComplete?.Invoke();

        
    }

    public void ScanAvatarConfig()
    {
        _avatarConfigs = new List<VRChatAvatar>();
        ScanAndProcessFiles(DefaultAvatarConfigPath);
    }

    private bool IsAvatarConfig(string id)
    {
        string pattern = @"avtr_[a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12}";
        return Regex.IsMatch(id, pattern);
    }
    
    private VRChatAvatar? ReadFile(string path)
    {
        VRChatAvatar? avatar = null;
        if (File.Exists(path))
        {
            try
            {
                string content = File.ReadAllText(path);
                avatar = JsonConvert.DeserializeObject<VRChatAvatar>(content);
            }
            catch 
            { }
        }

        return avatar;
    }
    
}