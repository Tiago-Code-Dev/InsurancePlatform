using ProposalService.Application.DTOs;
using ProposalService.Application.Integration;
using ProposalService.Application.Interfaces;
using ProposalService.Domain.Entities;
using ProposalService.Domain.Enums;
using ProposalService.Domain.Interfaces;
using ProposalService.Domain.ValueObjects;
using Shared.CrossCutting.Messaging.Events;
using Shared.CrossCutting.Response;

public class ProposalAppService : IProposalAppService
{
    private readonly IProposalRepository _proposalRepository;
    private readonly IProposalIntegrationEventPublisher _eventPublisher;

    public ProposalAppService(IProposalRepository proposalRepository, IProposalIntegrationEventPublisher eventPublisher)
    {
        _proposalRepository = proposalRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<CustomResponse<ProposalDto>> CreateAsync(CustomerDto customerDto, ContractDto contractDto)
    {
        try
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

            return CustomResponse<ProposalDto>.Created(MapToDto(proposal));
        }
        catch (Exception)
        {
            return CustomResponse<ProposalDto>.InternalServerError();
        }
    }

    public async Task<CustomResponse<ProposalDto>> GetByIdAsync(Guid id)
    {
        var proposal = await _proposalRepository.GetByIdAsync(id);
        if (proposal == null)
            return CustomResponse<ProposalDto>.Fail("Proposta não encontrada.");

        return CustomResponse<ProposalDto>.Ok(MapToDto(proposal));
    }

    public async Task<CustomResponse<Result>> ApproveAsync(Guid proposalId)
    {
        var proposal = await _proposalRepository.GetByIdAsync(proposalId);
        if (proposal == null)
            return CustomResponse<Result>.Fail("Proposta não encontrada.");

        proposal.Approve();
        await _proposalRepository.UpdateAsync(proposal);

        return CustomResponse<Result>.Ok(Result.Success());
    }

    public async Task<CustomResponse<Result>> RejectAsync(Guid proposalId)
    {
        var proposal = await _proposalRepository.GetByIdAsync(proposalId);
        if (proposal == null)
            return CustomResponse<Result>.Fail("Proposta não encontrada.");

        proposal.Reject();
        await _proposalRepository.UpdateAsync(proposal);

        return CustomResponse<Result>.Ok(Result.Success());
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

    public async Task<CustomResponse<IEnumerable<ProposalDto>>> GetAllAsync()
    {
        var proposals = await _proposalRepository.GetAllAsync();

        if (proposals == null || !proposals.Any())
            return CustomResponse<IEnumerable<ProposalDto>>.Ok(new List<ProposalDto>());

        var mapped = proposals.Select(MapToDto).ToList();

        return CustomResponse<IEnumerable<ProposalDto>>.Ok(mapped);
    }
}
