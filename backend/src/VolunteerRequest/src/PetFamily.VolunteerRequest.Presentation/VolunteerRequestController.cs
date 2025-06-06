using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.VolunteerRequest.Application.Commands.ApproveRequest;
using PetFamily.VolunteerRequest.Application.Commands.Create;
using PetFamily.VolunteerRequest.Application.Commands.EditRequest;
using PetFamily.VolunteerRequest.Application.Commands.RejectRequest;
using PetFamily.VolunteerRequest.Application.Commands.SendRequestToRevision;
using PetFamily.VolunteerRequest.Application.Commands.TakeRequestOnReview;
using PetFamily.VolunteerRequest.Application.Queries.GetAllSubmittedRequestsWithPagination;
using PetFamily.VolunteerRequest.Application.Queries.GetRequestsForCurrentAdmin;
using PetFamily.VolunteerRequest.Application.Queries.GetRequestsForCurrentUser;
using PetFamily.VolunteerRequest.Contracts.Requests;

namespace PetFamily.VolunteerRequest.Presentation;


public class VolunteerRequestController : ApplicationController
{
    [Authorize(Permissions.VolunteerRequests.CreateVolunteerRequest)]
    [HttpPost("{userId:guid}/submitting-application")]
    public async Task<ActionResult> CreateVolunteerRequest(
        [FromRoute] Guid userId,
        [FromBody] CreateVolunteerRequestRequest request,
        [FromServices] CreateVolunteerRequestHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateVolunteerRequestCommand(
            userId,
            request.FullName,
            request.Description,
            request.Email,
            request.Experience);
        
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [Authorize(Permissions.VolunteerRequests.TakeRequestOnReview)]
    [HttpPost("{requestId:guid}/{adminId:guid}/on-review")]
    public async Task<ActionResult> TakeRequestOnReview(
        [FromRoute] Guid requestId, Guid adminId,
        [FromServices] TakeRequestOnReviewHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new TakeRequestOnReviewCommand(requestId, adminId);
        
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok();
    }
    
    [Authorize(Permissions.VolunteerRequests.RejectRequest)]
    [HttpPost("{requestId:guid}/reject")]
    public async Task<ActionResult> RejectRequest(
        [FromRoute] Guid requestId,
        [FromBody] RejectRequestRequest request,
        [FromServices] RejectRequestHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new RejectRequestCommand(requestId, request.Description);
        
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok();
    }
    
    [Authorize(Permissions.VolunteerRequests.SendToRevision)]
    [HttpPost("{requestId:guid}/revision")]
    public async Task<ActionResult> SendToRevision(
        [FromRoute] Guid requestId,
        [FromBody] RevisionRequest request,
        [FromServices] SendRequestToRevisionHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new SendRequestToRevisionCommand(requestId, request.Description);
        
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok();
    }

    [Authorize(Permissions.VolunteerRequests.ApproveRequest)]
    [HttpPost("{requestId:guid}/approved")]
    public async Task<ActionResult> ApproveRequest(
        [FromRoute] Guid requestId,
        [FromBody] ApproveRequestRequest request,
        [FromServices] ApproveRequestHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new ApproveRequestCommand(requestId, request.PhoneNumber);

        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [Authorize(Permissions.VolunteerRequests.EditVolunteerRequest)]
        [HttpPost("{requestId:guid}/edit")]
        public async Task<ActionResult> EditRequest(
            [FromRoute] Guid requestId,
            [FromBody]  EditRequestRequest request,
            [FromServices] EditHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new EditCommand(
                requestId,
                request.Fio,
                request.Description,
                request.Email,
                request.Experience);
        
            var result = await handler.HandleAsync(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();
        
            return Ok();
        }
        
    [Authorize(Permissions.VolunteerRequests.GetAllSubmittedVolunteerRequest)]
    [HttpGet]
    public async Task<ActionResult> GetAllSubmittedRequests(
        [FromQuery]  GetAllSubmittedRequestsRequest request,
        [FromServices] GetAllSubmittedRequestHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new GetAllSubmittedRequestQuery(request.Page, request.PageSize);
        
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [Authorize(Permissions.VolunteerRequests.GetRequestsForCurrentAdmin)]
    [HttpGet("/requests-admin/{adminId:guid}")]
    public async Task<ActionResult> GetRequestsForCurrentAdmin(
        [FromRoute] Guid adminId,
        [FromQuery]  GetRequestsForCurrentAdminRequest request,
        [FromServices] GetRequestsForCurrentAdminHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new GetRequestsForCurrentAdminQuery(
            adminId,
            request.Status,
            request.Page,
            request.PageSize);
        
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [Authorize(Permissions.VolunteerRequests.GetRequestsForCurrentUser)]
    [HttpGet("/requests-user/{userId:guid}")]
    public async Task<ActionResult> GetRequestsForCurrentUser(
        [FromRoute] Guid userId,
        [FromQuery]  GetRequestsForCurrentUserRequest request,
        [FromServices] GetRequestsForCurrentUserHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new GetRequestsForCurrentUserQuery(
            userId,
            request.Status,
            request.Page,
            request.PageSize);
        
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}