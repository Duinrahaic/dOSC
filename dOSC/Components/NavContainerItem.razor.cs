﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace dOSC.Components
{
	public partial class NavContainerItem
	{
		[Parameter]
		public NavItem? Item { get; set; }
		[Parameter]
		public bool IsActive { get; set; }
		[Parameter]
		public EventCallback<NavItem> OnClick { get; set; }
		private bool Visible => Item != null;
		private string AppCSS => GetNavItemCSS();

		[Inject]
		private IJSRuntime js { get; set; }

		private async Task OnClickEvent(MouseEventArgs e)
		{
			if(OnClick.HasDelegate)
			{
				
				await OnClick.InvokeAsync(Item);
			}
		}


		private string GetNavItemCSS()
		{
			List<string> Classes = new();
			if(Visible)
			{
				switch (Item?.Type)
				{
					case NavItemType.App:
						Classes.Add("purple-app");
						break;
					case NavItemType.System:
						Classes.Add("green-app");
						break;
				}
				if (IsActive)
				{
					Classes.Add("squircle-active");
				}
				else
				{
					Classes.Add("squircle");
				}
			}
			else
			{
				Classes.Add("squircle");
				Classes.Add("purple-app");
			}
			return string.Join(" ", Classes);
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				try
				{
                    await js.InvokeVoidAsync("addTooltips");

                }
                catch (Exception ex)
				{

				}
			}
		}
	}
}
