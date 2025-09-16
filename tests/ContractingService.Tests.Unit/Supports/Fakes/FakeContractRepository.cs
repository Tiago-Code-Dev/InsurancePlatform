using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using ContractingService.Domain.Entities;
using ContractingService.Domain.Interfaces;

namespace ContractingService.Tests.Unit.Support.Fakes
{
    public sealed class FakeContractRepository : IContractRepository
    {
        private readonly ConcurrentDictionary<Guid, Contract> _store = new();
        public int UpdateCount { get; private set; }
        public int AddCount { get; private set; }

        public Task<Contract?> GetByIdAsync(Guid id)
        {
            _store.TryGetValue(id, out var c);
            return Task.FromResult(c);
        }

        public Task AddAsync(Contract contract)
        {
            _store[contract.Id] = contract;
            AddCount++;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Contract contract)
        {
            _store[contract.Id] = contract;
            UpdateCount++;
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(Guid id)
            => Task.FromResult(_store.ContainsKey(id));

        public Contract? Peek(Guid id) => _store.TryGetValue(id, out var c) ? c : null;
        public void ClearCounters() { UpdateCount = 0; AddCount = 0; }
    }
}
