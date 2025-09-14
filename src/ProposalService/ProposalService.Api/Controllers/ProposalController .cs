using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProposalService.Api.Contracts.Requests;
using ProposalService.Application.Interfaces;
using Shared.CrossCutting.Auth;
using Shared.CrossCutting.Controllers;
using Shared.CrossCutting.Notifications;

namespace ProposalService.API.Controllers
{
    [Route("api/v1/proposals")]
    public class ProposalsController : MainController
    {
        private readonly IProposalAppService _proposalAppService;

        public ProposalsController(
            IProposalAppService proposalAppService,
            INotifier notifier) : base(notifier)
        {
            _proposalAppService = proposalAppService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProposalRequest request)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var result = await _proposalAppService.CreateAsync(request.Customer, request.Contract);
            return CustomResponse(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _proposalAppService.GetAllAsync();
            return CustomResponse(result);
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _proposalAppService.GetByIdAsync(id);

            if (result is null)
            {
                NotifyError("Proposal not found.");
                return CustomResponse();
            }

            return CustomResponse(result);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id:guid}/approve")]
        public async Task<IActionResult> Approve(Guid id)
        {
            await _proposalAppService.ApproveAsync(id);
            return CustomResponse(new { message = "Proposal approved successfully." });
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id:guid}/reject")]
        public async Task<IActionResult> Reject(Guid id)
        {
            await _proposalAppService.RejectAsync(id);
            return CustomResponse(new { message = "Proposal rejected successfully." });
        }
    }
}
