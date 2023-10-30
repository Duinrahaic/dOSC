using dOSCEngine.Services;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Components
{
    public partial class NavContainer
    {
        [Parameter]
        public List<NavItem> Apps { get; set; } = new();
        [Parameter]
        public NavItem? SelectedItem { get; set; }
        [Inject]
        public NavigationManager? NM { get; set; }
        [Inject]
        public dOSCService? Engine { get; set; }
        [Parameter]
        public EventCallback<NavItem> SelectedItemChanged { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                string route = NM.Uri.Replace(NM.BaseUri, "");
                if (route.ToLower().StartsWith("apps"))
                {
                    SelectedItem = Apps.FirstOrDefault(x => x.Name.ToLower() == "apps");

                }
                else if (route.ToLower().StartsWith("settings"))
                {
                    SelectedItem = Apps.FirstOrDefault(x => x.Name.ToLower() == "settings");
                }
                else
                {
                    SelectedItem = Apps.FirstOrDefault();

                }

            }
        }

        private async Task OnNavItemSelected(NavItem item)
        {
            SelectedItem = item;
            if (SelectedItemChanged.HasDelegate)
            {

                await SelectedItemChanged.InvokeAsync(item);
            }
            NM!.NavigateTo($"{item.Navigation}");
        }
    }
}
