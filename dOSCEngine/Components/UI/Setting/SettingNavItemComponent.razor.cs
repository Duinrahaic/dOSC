﻿using Microsoft.AspNetCore.Components;

namespace dOSCEngine.Components.UI.Setting
{
    public partial class SettingNavItemComponent
    {
        [Parameter]
        public string Title { get; set; } = string.Empty;
        [Parameter]
        public string Description { get; set; } = string.Empty;
        [Parameter]
        public string Icon { get; set; } = string.Empty;

        [Parameter]
        public EventCallback<string> OnNavItemSelected { get; set; }
        [Parameter]
        public string? Selected { get; set; } = null;


        private async Task OnClick(EventArgs e)
        {
            if (OnNavItemSelected.HasDelegate)
            {
                await OnNavItemSelected.InvokeAsync(Title);
            }
        }

    }
}
