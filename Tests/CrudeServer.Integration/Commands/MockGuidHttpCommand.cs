﻿using CrudeServer.HttpCommands;
using CrudeServer.HttpCommands.Contract;

namespace CrudeServer.Integration.Commands
{
    public class MockGuidHttpCommand : HttpCommand
    {
        protected override async Task<IHttpResponse> Process()
        {
            return await View("simple.html", new
            {
                value = "Yoh " + Guid.NewGuid()
            });
        }
    }
}