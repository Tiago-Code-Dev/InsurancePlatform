using Microsoft.EntityFrameworkCore;
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

            if (!Enum.TryParse<ContractType>(contractDto.Type, out var contractType))
                return CustomResponse<ProposalDto>.Fail($"Invalid contract type: {contractDto.Type}");

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
        try
        {
            var proposal = await _proposalRepository.GetByIdAsync(id);

            return proposal is null
                ? CustomResponse<ProposalDto>.Fail("Proposal not found.")
                : CustomResponse<ProposalDto>.Ok(MapToDto(proposal));

        }catch
        {  return CustomResponse<ProposalDto>.InternalServerError();}
     }

    public async Task<CustomResponse<IEnumerable<ProposalDto>>> GetAllAsync()
    {
        try
        {
            var proposals = await _proposalRepository.GetAllAsync();
            var mapped = proposals.Select(MapToDto).ToList();
            return CustomResponse<IEnumerable<ProposalDto>>.Ok(mapped);
        }
        catch
        {
            return CustomResponse<IEnumerable<ProposalDto>>.InternalServerError();
        }
    }

    public Task<CustomResponse<Result>> ApproveAsync(Guid proposalId) =>
     ChangeStatusAsync(proposalId, p => p.Approve(), "Proposal successfully approved..");

    public Task<CustomResponse<Result>> RejectAsync(Guid proposalId) =>
       ChangeStatusAsync(proposalId, p => p.Reject(), "Proposal successfully rejected.");

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

    private async Task<CustomResponse<Result>> ChangeStatusAsync( Guid proposalId,Action<Proposal> action,string successMessage)
    {
        try
        {
            var proposal = await _proposalRepository.GetByIdAsync(proposalId);
            if (proposal is null)
                return CustomResponse<Result>.Fail("Proposal not found.");

            action(proposal);
            await _proposalRepository.UpdateAsync(proposal);

            return CustomResponse<Result>.Ok(Result.Success(successMessage));
        }
        catch (ArgumentException ex)
        {
            return CustomResponse<Result>.Fail(ex.Message);
        }
        catch
        {
            return CustomResponse<Result>.InternalServerError();
        }
    }
}
