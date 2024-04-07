using System.Text;
using dOSC.Shared.Models.Wiresheet;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace dOSC.Client.Utilities;

public static class JSHelpers
{
    public static async Task DownloadApp(this IJSRuntime js, dOSCDataDTO appData)
    {
        if (appData == null) return;
        await js.InvokeVoidAsync("GenerateToasterMessage", "Sent app to to downloads folder!");
        var json = JsonConvert.SerializeObject(appData, Formatting.Indented);
        var filename = $"app-{appData.AppGuid}.json";
        var data = Encoding.UTF8.GetBytes(json);
        await js.InvokeAsync<object>("saveAsFile", filename, Convert.ToBase64String(data));
    }
}