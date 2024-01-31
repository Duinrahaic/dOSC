using System.Diagnostics;
using CefSharp;
using CefSharp.Handler;

namespace dOSC.Helpers;

public class RequestHandlerHelper : RequestHandler
{
    protected override bool OnCertificateError(IWebBrowser chromiumWebBrowser, IBrowser browser, CefErrorCode errorCode,
        string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
    {
        callback.Continue(true);
        return true;
    }

    protected override bool OnBeforeBrowse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool userGesture,
        bool isRedirect)
    {
        if (!request.Url.Contains("localhost",StringComparison.CurrentCultureIgnoreCase))
        {
            Process.Start(new ProcessStartInfo(request.Url) { UseShellExecute = true });
            return true;
        }
        return false; // Allow the request to proceed.
    }    
}