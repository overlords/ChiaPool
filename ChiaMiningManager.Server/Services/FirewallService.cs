using ChiaMiningManager.Models;
using Common.Services;
using IPTables.Net;
using IPTables.Net.Iptables;
using IPTables.Net.Iptables.Adapter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SystemInteract.Local;

namespace ChiaMiningManager.Services
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
            await RefreshMinerIPsAsync();

        }

        protected override async ValueTask RunAsync()
        {
            while (true)
            {
                await Task.Delay(IPRefreshDelay);
                await RefreshMinerIPsAsync();
            }
        }

        private async Task RefreshMinerIPsAsync()
        {
            await AccessSemaphore.WaitAsync();
            FlushIPsInternal();

            try
            {
                using var scope = Provider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<MinerContext>();
                using var adapter = System.GetTableAdapter(4);
                var miners = await dbContext.Miners.ToListAsync();

                var rules = new List<string>();

                foreach (var miner in miners)
                {
                    rules.Add(GetAcceptRule(miner.Address));
                }

                var ruleSet = new IpTablesRuleSet(4, rules, System);
                var chain = System.GetChain(adapter, IpTable, IpChain);

                foreach (var rule in ruleSet.Rules)
                {
                    chain.AddRule(rule);
                }
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
            var chain = System.GetChain(adapter, IpTable, IpChain) as IpTablesChain;
            var rules = System.GetRules(adapter, IpTable, IpChain);

            foreach (var rule in rules)
            {
                chain.DeleteRule(rule as IpTablesRule);
            }
        }
        private void AcceptIPInternal(IPAddress address)
        {
            using var adapter = System.GetTableAdapter(4);
            var chain = System.GetChain(adapter, IpTable, IpChain) as IpTablesChain;

            var rule = new IpTablesRule(System, chain);
            rule.AppendToRule(GetAcceptRule(address));

            chain.AddRule(rule);
        }
        private void DropIPInternal(IPAddress address)
        {
            using var adapter = System.GetTableAdapter(4);
            var chain = System.GetChain(adapter, IpTable, IpChain) as IpTablesChain;

            var rule = chain.Rules.FirstOrDefault(x => x.GetCommand().Contains($"{address}"));

            if (rule == null)
            {
                return;
            }

            chain.DeleteRule(rule);
        }

        private string GetAcceptRule(IPAddress address)
            => $"-A {IpChain} -s {address} -j ACCEPT";
    }
}
