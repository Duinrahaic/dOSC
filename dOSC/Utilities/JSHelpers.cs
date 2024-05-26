using dOSC.Component.Wiresheet;
using LiveSheet;
using Microsoft.JSInterop;

namespace dOSC.Utilities;

public static class JSHelpers
{
    public static async Task DownloadApp(this IJSRuntime js, WiresheetDiagram data)
    {
        await js.InvokeVoidAsync("GenerateToasterMessage", "Sent app to to downloads folder!");
        var json = data.SerializeLiveSheet();
        var filename = $"app-{data.Guid}.json";
        await js.InvokeAsync<object>("saveAsFile", filename, json );
    }
}