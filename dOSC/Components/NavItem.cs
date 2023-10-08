using dOSC.Services;

namespace dOSC.Components
{
	public class NavItem
	{
		public string Name { get; set; }
		public string Icon { get; set; } = string.Empty;
		public dOSCWiresheet? Wiresheet { get; set; } = new(new());
		public NavItemType Type => this.Wiresheet == null ? NavItemType.System : NavItemType.App;


		public NavItem(string Name, string Icon)
		{
            this.Name = Name;
            this.Icon = Icon;
        }
		public NavItem(string Name, string Icon, dOSCWiresheet Wiresheet)
		{
            this.Name = Name;
            this.Icon = Icon;
            this.Wiresheet = Wiresheet;
        }
	}
}
