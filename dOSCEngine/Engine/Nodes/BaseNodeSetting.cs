using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dOSCEngine.Engine.Nodes
{
    public class BaseNodeSetting
    {
        [StringLength(maximumLength: 125)]
        [RegularExpression(@"^[^,:*?""<>\|]*$", ErrorMessage = @"Cannot use illegal characters :*?""<>\|")]
        public string DisplayName { get; set; } = string.Empty;

        public string Name { get; set; } = "Unknown";


    }
}
