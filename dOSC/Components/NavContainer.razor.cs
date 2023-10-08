using dOSC.Services;
using Microsoft.AspNetCore.Components;

namespace dOSC.Components
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
        public dOSCEngine? Engine { get; set; }
        [Parameter]
		public EventCallback<NavItem> SelectedItemChanged { get; set; }

        protected override bool ShouldRender() => false;
        private async Task OnNavItemSelected(NavItem item)
		{
			SelectedItem = item;
			if (SelectedItemChanged.HasDelegate)
			{
				
                await SelectedItemChanged.InvokeAsync(item);
			}
            if(Engine != null)
            {
                if (item.Wiresheet != null)
                {
                    // Page has to clear before being able to render another another page. Supply Empty GUID to clear
                    NM!.NavigateTo($"app/{Guid.Empty}", forceLoad: false); 
                    NM!.NavigateTo($"app/{item.Wiresheet.AppGuid}", forceLoad: false);
                }
            }
        }
    }
}
