using dOSC.Component.Wiresheet;
using LiveSheet;
using Microsoft.JSInterop;

namespace dOSC.Utilities;

public static class JsHelpers
{
    public static async Task DownloadApp(this IJSRuntime js, WiresheetDiagram data)
    {
        try
        {
            var json = data.SerializeLiveSheet();
            await js.InvokeAsync<object>("saveAsFile", $"app-{data.Guid}.json", json );
            await js.InvokeVoidAsync("GenerateToasterMessage", "Sent app to to downloads folder!");
        }
        catch(Exception ex)
        {
            // ignore 
        }

        
    }
}