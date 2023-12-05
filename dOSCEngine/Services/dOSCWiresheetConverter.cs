using dOSCEngine.Engine.Nodes;
using dOSCEngine.Engine.Nodes.Connector.OSC;
using dOSCEngine.Engine.Nodes.Constant;
using dOSCEngine.Engine.Nodes.Math;
using dOSCEngine.Engine.Nodes.Utility;

namespace dOSCEngine.Services
{
    public partial class dOSCWiresheet
    {
        public dOSCWiresheet()
        {
            AppGuid = Guid.NewGuid();
            Diagram = new(dOSCWiresheetConfiguration.Options);
            Diagram.RegisterBlocks();
            // Diagram.SuspendRefresh = true;
        }

        public dOSCWiresheet(Guid AppGuid)
        {
            Diagram = new(dOSCWiresheetConfiguration.Options);
            Diagram.RegisterBlocks();
            // Diagram.SuspendRefresh = true;
            this.AppGuid = AppGuid;
        }


        public dOSCWiresheet(dOSCWiresheetDTO dto)
        {
            Diagram = new(dOSCWiresheetConfiguration.Options);
            Diagram.RegisterBlocks();
            AppGuid = dto.AppGuid;
            AppName = dto.AppName;
            AppVersion = dto.AppVersion;
            AppDescription = dto.AppDescription;
            Created = dto.Created;
            Modified = dto.Modified;
            AppIcon = dto.AppIcon;
            //this.Diagram.SuspendRefresh = false;
        }

        public dOSCWiresheetDTO GetDTO()
        {
            return new dOSCWiresheetDTO(this);
        }

        
    }
}
