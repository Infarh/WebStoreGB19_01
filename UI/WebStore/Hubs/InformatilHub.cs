using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace WebStore.Hubs
{
    public class InformatilHub : Hub
    {
        public async Task Send(string Message) => await Clients.All.SendAsync("Send", Message);
    }
}
