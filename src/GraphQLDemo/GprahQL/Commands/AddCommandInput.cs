﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.GprahQL.Commands
{
    public record AddCommandInput(string HowTo, string CommandLine, int PlatformId);
}
