using dOSC.Components;
using dOSC.Services;
using Microsoft.AspNetCore.Components;

namespace dOSC.Pages
{
    public partial class Index 
    {
        [Inject]
        private dOSCEngine? Engine { get; set; }
        private dOSCWiresheet? ActiveWiresheet;

    }
}
