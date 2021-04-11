using Common.Services;
using IPTables.Net;
using IPTables.Net.Iptables;
using IPTables.Net.Iptables.Adapter;
using IPTables.Net.Netfilter.TableSync;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SystemInteract.Local;

namespace ChiaPool.Services
{
    public class FirewallService : Service
    {
        private const string IpTable = "filter";
        private const string IpChain = "WHITE";

        private const int IPRefreshDelay = 60 * 60 * 1000; //1 Hour

        private readonly SemaphoreSlim AccessSemaphore;
        private readonly IpTablesSystem System;

        public FirewallService()
        {
            var ad = new IPTablesBinaryAdapter();
            var fa = new LocalFactory();
            System = new IpTablesSystem(fa, ad);

            AccessSemaphore = new SemaphoreSlim(1, 1);
        }

        protected override async ValueTask InitializeAsync()
        {
            await AccessSemaphore.WaitAsync();
            try
            {
                FlushIPsInternal();
            }
            finally
            {
                AccessSemaphore.Release();
            }
        }
        public async Task AcceptIPAsync(IPAddress address)
        {
            await AccessSemaphore.WaitAsync();
            try
            {
                AcceptIPInternal(address);
            }
            finally
            {
                AccessSemaphore.Release();
            }
        }
        public async Task DropIPAsync(IPAddress address)
        {
            await AccessSemaphore.WaitAsync();
            try
            {
                DropIPInternal(address);
            }
            finally
            {
                AccessSemaphore.Release();
            }
        }
        public async Task SwapMinerIP(IPAddress oldAddress, IPAddress newAddress)
        {
            await AccessSemaphore.WaitAsync();
            try
            {
                AcceptIPInternal(newAddress);
                DropIPInternal(oldAddress);
            }
            finally
            {
                AccessSemaphore.Release();
            }
        }

        private void FlushIPsInternal()
        {
            using var adapter = System.GetTableAdapter(4);
            var sync = new DefaultNetfilterSync<IpTablesRule>();
            (System.GetChain(adapter, IpTable, IpChain) as IpTablesChain).Sync(adapter, Enumerable.Empty<IpTablesRule>(), sync);
        }
        private void AcceptIPInternal(IPAddress address)
        {
            if (address == null)
            {
                return;
            }

            using var adapter = System.GetTableAdapter(4);

            var chain = new IpTablesChain(IpTable, IpChain, 4, System);
            var rule = new IpTablesRule(System, chain);
            rule.AppendToRule(GetAcceptRule(address));
            chain.AddRule(rule);

            var sync = new DefaultNetfilterSync<IpTablesRule>();
            (System.GetChain(adapter, IpTable, IpChain) as IpTablesChain).Sync(adapter, chain.Rules, sync);
            Logger.LogInformation($"Whitelisted {address}");
        }
        private void DropIPInternal(IPAddress address)
        {
            if (address == null)
            {
                return;
            }

            using var adapter = System.GetTableAdapter(4);
            var chain = System.GetChain(adapter, IpTable, IpChain) as IpTablesChain;

            var rule = chain.Rules.FirstOrDefault(x => x.GetCommand().Contains($"{address}"));

            if (rule == null)
            {
                return;
            }

            chain.DeleteRule(rule);
            var sync = new DefaultNetfilterSync<IpTablesRule>();
            (System.GetChain(adapter, IpTable, IpChain) as IpTablesChain).Sync(adapter, chain.Rules, sync);
            Logger.LogInformation($"Blacklisted {address}");
        }

        private string GetAcceptRule(IPAddress address)
            => $"-A {IpChain} -s {address} -j ACCEPT";
    }
}
