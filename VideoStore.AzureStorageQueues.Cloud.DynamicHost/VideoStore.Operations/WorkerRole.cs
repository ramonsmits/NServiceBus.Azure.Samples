using System;
using Microsoft.WindowsAzure.ServiceRuntime;
using NServiceBus.Hosting.Azure;

namespace VideoStore.Operations
{
    public class WorkerRole : RoleEntryPoint
    {
        private NServiceBusRoleEntrypoint nsb = new NServiceBusRoleEntrypoint();

        public override bool OnStart()
        {
            nsb.Start();

            return base.OnStart();
        }

        public override void OnStop()
        {
            nsb.Stop();

            base.OnStop();
        }
    }
}