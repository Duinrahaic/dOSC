using dOSC.Services;

namespace dOSC.Components
{
	public class NavItem
	{
		public string Name { get; set; }
		public string Icon { get; set; } = string.Empty;
		public string Navigation { get; set; } = string.Empty;
		public NavItemType Type { get; set; }


		public NavItem(string Name, string Icon, string Navigation, NavItemType Type)
		{
            this.Name = Name;
            this.Icon = Icon;
			this.Navigation = Navigation;
			this.Type = Type;
        }
		
	}
}
