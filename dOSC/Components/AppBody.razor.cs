using Microsoft.AspNetCore.Components;

namespace dOSC.Components
{
	public partial class AppBody
	{
		[Parameter]
		public RenderFragment? ChildContent { get; set; } 
	}
}
