﻿using dOSC.Engine.Nodes;
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
            BlazorDiagram = new(dOSCWiresheetConfiguration.Options);
            BlazorDiagram.RegisterBlocks();
            BlazorDiagram.SuspendRefresh = true;
            Build();
        }

        public dOSCWiresheet(Guid AppGuid)
        {
            BlazorDiagram = new(dOSCWiresheetConfiguration.Options);
            BlazorDiagram.RegisterBlocks();
            BlazorDiagram.SuspendRefresh = true;
            this.AppGuid = AppGuid;
            Build();
        }


        public dOSCWiresheet(dOSCWiresheetDTO dto)
        {
            BlazorDiagram = new(dOSCWiresheetConfiguration.Options);
            BlazorDiagram.RegisterBlocks();
            AppGuid = dto.AppGuid;
            AppName = dto.AppName;
            AppVersion = dto.AppVersion;
            AppDescription = dto.AppDescription;
            Created = dto.Created;
            Modified = dto.Modified;
            IsPlaying = dto.Running;
            Build();
        }

        public dOSCWiresheetDTO GetDTO()
        {
            return new dOSCWiresheetDTO(this);
        }

        
    }
}
