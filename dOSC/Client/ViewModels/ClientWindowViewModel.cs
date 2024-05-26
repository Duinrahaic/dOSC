using System;
using ReactiveUI;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using System.ComponentModel.DataAnnotations;
using Avalonia.Threading;
 using static System.Net.WebRequestMethods;

namespace dOSC.Client.ViewModels;

public class ClientWindowViewModel : ViewModelBase
{
    public ReactiveCommand<Unit, Unit> ExitCommand { get; }
    public Interaction<Unit, Unit> ExitInteraction { get; }
    

    public ClientWindowViewModel()
    {
        if(!Avalonia.Controls.Design.IsDesignMode)
        {
        }
        ExitCommand = ReactiveCommand.CreateFromTask(OnExit);
        ExitInteraction = new Interaction<Unit, Unit>();
    }

    private async Task OnExit()
    {
        await ExitInteraction.Handle(Unit.Default);

    }
    
}
