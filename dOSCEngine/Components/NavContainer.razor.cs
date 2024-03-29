﻿using dOSCEngine.Services;
using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Components
{
    public partial class NavContainer : IDisposable
    {
        [Parameter]
        public List<NavItem> Apps { get; set; } = new();
        [Parameter]
        public NavItem? SelectedItem { get; set; }
        [Inject]
        public NavigationManager? NM { get; set; }
        [Inject]
        public dOSCService? Engine { get; set; }


        protected override void OnInitialized()
        {
            NM!.LocationChanged += NavContainer_LocationChanged;
        }

        private void NavContainer_LocationChanged(object? sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
        {
            Update();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                Update();
            }
        }

        private void Update()
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
            else if (route.ToLower().StartsWith("editor"))
            {
                SelectedItem = Apps.FirstOrDefault(x => x.Name.ToLower() == "editor");
            }
            else
            {
                SelectedItem = Apps.FirstOrDefault();
            }
            StateHasChanged();
        }


        private void OnNavItemSelected(NavItem item)
        {

            if (SelectedItem?.Name == item.Name)
                return;
            Update();
            NM!.NavigateTo($"{item.Navigation}");
        }

        public void Dispose()
        {
            NM!.LocationChanged -= NavContainer_LocationChanged;
        }
    }
}
