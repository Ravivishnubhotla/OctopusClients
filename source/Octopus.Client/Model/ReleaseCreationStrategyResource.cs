﻿using System;

namespace Octopus.Client.Model
{
    public class ReleaseCreationStrategyResource
    {
        public PackageReferenceByActionIdResource ReleaseCreationPackageStepId { get; set; }

        public string ChannelId { get; set; }
    }
}