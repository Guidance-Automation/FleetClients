﻿using FleetClients.Core.FleetManagerServiceReference;
using System;

namespace FleetClients.Core
{
    public class FleetManagerServiceCallback : IFleetManagerService_PublicAPI_v2_0Callback
    {
        public FleetManagerServiceCallback()
        {
        }

        public event Action<FleetState> FleetStateUpdate;

        public void OnCallback(FleetState fleetState)
        {
            Action<FleetState> handlers = FleetStateUpdate;

            if (handlers != null)
            {
                foreach (Action<FleetState> handler in handlers.GetInvocationList())
                {
                    handler.BeginInvoke(fleetState, null, null);
                }
            }
        }
    }
}