﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pash.ParserIntrinsics.Nodes
{
    public class command_name_node : _node
    {
        internal override void Execute(Implementation.ExecutionContext context, System.Management.Automation.ICommandRuntime commandRuntime)
        {
            throw new NotImplementedException();
        }

        internal override object GetValue(Implementation.ExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}