using HouseKeeperApi.Models;
using HouseKeeperApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseKeeperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Tenat, Landlord")]
    public class IssueController : ControllerBase
    {
        private readonly IIssueService _issueService;

        public IssueController(IIssueService issueService)
        {
            _issueService = issueService;
        }

        [HttpGet("{issueId}")]
        public async Task<ActionResult<IssueDto>> GetIssueById([FromRoute] int issueId)
        {
            var issue = await _issueService.GetIssueById(issueId);

            return issue == null ? NotFound() : Ok(issue);
        }

        [HttpGet("byUserId/{userId}")]
        public async Task<ActionResult<IssueDto>> GetIssuesByUserId([FromRoute] int userId)
        {
            var issues = await _issueService.GetIssuesByUserId(userId);

            return issues == null ? NotFound() : Ok(issues);
        }

        [HttpPost]
        public async Task<ActionResult> CreateIssue([FromBody] IssueDto issueDto)
        {
            var issueId = await _issueService.CreateIssue(issueDto);
            return Created($"/api/issue/{issueId}", null);

        }

        [HttpPut("{issueId}")]
        public async Task<ActionResult> UpdateIssue([FromRoute] int issueId, [FromBody] IssueDto issueDto)
        {
            var issue = await _issueService.UpdateIssue(issueId, issueDto);
            return issue ? Ok() : NotFound();
        }

        [HttpDelete("{issueId}")]
        public async Task<ActionResult> DeleteIssue([FromRoute] int issueId)
        {
            var issue = await _issueService.DeleteIssue(issueId);
            return issue ? NoContent() : NotFound();
        }

    }
}
