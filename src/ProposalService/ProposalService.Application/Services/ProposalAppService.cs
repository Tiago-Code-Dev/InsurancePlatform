namespace ProposalService.Application.Services;

using ProposalService.Application.DTOs;
using ProposalService.Application.Interfaces;
using ProposalService.Application.Commands;
using ProposalService.Domain.Entities;
using ProposalService.Domain.Interfaces;
using ProposalService.Domain.ValueObjects;
using ProposalService.Domain.Enums;
using ProposalService.Application.Integration;
using Shared.Contracts.Events;

public class ProposalAppService : IProposalAppService
{
    private readonly IProposalRepository _proposalRepository;
    private readonly IProposalIntegrationEventPublisher _eventPublisher;

    public ProposalAppService(IProposalRepository proposalRepository, IProposalIntegrationEventPublisher eventPublisher)
    {
        _proposalRepository = proposalRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<ProposalDto> CreateAsync(CustomerDto customerDto, ContractDto contractDto)
    {
        var customer = new Customer(
            customerDto.Name,
            new Document(customerDto.Document),
            new Email(customerDto.Email)
        );

        var contract = new Contract(
            Enum.Parse<ContractType>(contractDto.Type),
            new Money(contractDto.Premium),
            contractDto.StartDate,
            contractDto.EndDate
        );

        var proposal = new Proposal(customer, contract);

        await _proposalRepository.AddAsync(proposal);

        var proposalEvent = new ProposalCreatedEvent
        {
            ProposalId = proposal.Id,
            CustomerName = proposal.Customer.Name,
            Amount = proposal.Contract.Premium.Amount,
            CreatedAt = DateTime.UtcNow
        };

        await _eventPublisher.PublishAsync(proposalEvent);

        return MapToDto(proposal);
    }

    public async Task<ProposalDto?> GetByIdAsync(Guid id)
    {
        var proposal = await _proposalRepository.GetByIdAsync(id);
        return proposal == null ? null : MapToDto(proposal);
    }

    public async Task ApproveAsync(Guid proposalId)
    {
        var proposal = await _proposalRepository.GetByIdAsync(proposalId);
        if (proposal == null) return;

        proposal.Approve();
        await _proposalRepository.UpdateAsync(proposal);
    }

    public async Task RejectAsync(Guid proposalId)
    {
        var proposal = await _proposalRepository.GetByIdAsync(proposalId);
        if (proposal == null) return;

        proposal.Reject();
        await _proposalRepository.UpdateAsync(proposal);
    }

    private static ProposalDto MapToDto(Proposal proposal) =>
        new()
        {
            Id = proposal.Id,
            CustomerName = proposal.Customer.Name,
            CustomerDocument = proposal.Customer.Document.Number,
            ContractType = proposal.Contract.Type.ToString(),
            Premium = proposal.Contract.Premium.Amount,
            Status = proposal.Status.ToString()
        };
}
