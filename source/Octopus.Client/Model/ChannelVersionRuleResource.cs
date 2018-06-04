using System.Collections.Generic;
using Octopus.Client.Extensibility.Attributes;

namespace Octopus.Client.Model
{
    public class ChannelVersionRuleResource : Resource
    {
        [Writeable]
        public ICollection<PackageReferenceByActionNameResource> Actions { get; set; }

        [Writeable]
        [Trim]
        public string VersionRange { get; set; }

        [Writeable]
        [Trim]
        public string Tag { get; set; }
    }
}