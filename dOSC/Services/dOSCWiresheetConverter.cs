using dOSC.Engine.Nodes;
using dOSC.Engine.Nodes.Connector.OSC;
using dOSC.Engine.Nodes.Constant;
using dOSC.Engine.Nodes.Math;
using dOSC.Engine.Nodes.Utility;

namespace dOSC.Services
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
            //this.Diagram.SuspendRefresh = false;
        }

        public dOSCWiresheetDTO GetDTO()
        {
            return new dOSCWiresheetDTO(this);
        }

        
    }
}
