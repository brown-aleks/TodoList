using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using ToDoList.API.Data;
using ToDoList.API.Models;

namespace ToDoList.API.Services
{
    public class OpenLoopService
    {
        private readonly ILogger<OpenLoopService> logger;
        private readonly OpenLoopDbContext openLoopDbContext;
        private readonly IAuthorizationService authService;

        public OpenLoopService(
            ILogger<OpenLoopService> logger,
            OpenLoopDbContext openLoopDbContext,
            IAuthorizationService authService)
        {
            this.logger = logger;
            this.openLoopDbContext = openLoopDbContext;
            this.authService = authService;
        }
        public async Task<List<OpenLoop>> GetAsync()
        {
            return await openLoopDbContext.OpenLoop
                .Where(r => !r.IsDeleted)
                .ToListAsync();
        }
        public async Task<OpenLoop?> GetAsync(Guid id)
        {
            return await openLoopDbContext.OpenLoop
                .Where(x => x.Id == id)
                .Where(r => !r.IsDeleted)
                .SingleOrDefaultAsync();
        }
        public async Task<OpenLoop?> CreateAsync(OpenLoopRequest openLoopRequest, ClaimsPrincipal userСreator)
        {
            var userId = userСreator.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (userId.IsNullOrEmpty())
            {
                return null;
            }

            OpenLoop openLoop = new()
            {
                Id = Guid.NewGuid(),
                CreatedDateUtc = DateTime.UtcNow,
                Note = openLoopRequest.Note,
                Description = openLoopRequest.Description,
                Complete = openLoopRequest.Complete,
                CreatorId = userId!
            };

            bool successPars = DateTimeOffset.TryParse(openLoopRequest.CompleteDate, out DateTimeOffset dateTimeOffset);
            openLoop.CompleteDateUtc = successPars ? dateTimeOffset.UtcDateTime : DateTime.MinValue;

            openLoopDbContext.Add(openLoop);
            await openLoopDbContext.SaveChangesAsync();
            return openLoop;
        }
        public async Task<IActionResult> UpdateAsync(OpenLoop openLoop, ClaimsPrincipal userModifying)
        {
            var openLoopToUpdate = await openLoopDbContext.OpenLoop.FindAsync(openLoop.Id);
            if (openLoopToUpdate == null || openLoopToUpdate.IsDeleted)
            {
                return new NotFoundResult();
            }

            var authResult = await authService.AuthorizeAsync(userModifying, openLoopToUpdate, "CanEditOpenLoop");
            if (!authResult.Succeeded)
            {
                return new ForbidResult();
            }

            openLoopToUpdate.Description = openLoop.Description;
            openLoopToUpdate.Note = openLoop.Note;
            openLoopToUpdate.CompleteDateUtc = openLoop.CompleteDateUtc;
            openLoopToUpdate.Complete = openLoop.Complete;

            openLoopDbContext.OpenLoop.Update(openLoopToUpdate);
            await openLoopDbContext.SaveChangesAsync();

            return new NoContentResult();
        }
        public async Task<IActionResult> DeleteAsync(Guid id, ClaimsPrincipal userDeleting)
        {
            var openLoopToDelete = await openLoopDbContext.OpenLoop.FindAsync(id);
            if (openLoopToDelete == null)
            {
                return new NotFoundResult();
            }

            var authResult = await authService.AuthorizeAsync(userDeleting, openLoopToDelete, "CanEditOpenLoop");
            if (!authResult.Succeeded)
            {
                return new ForbidResult();
            }

            openLoopToDelete.IsDeleted = true;

            openLoopDbContext.OpenLoop.Update(openLoopToDelete);
            await openLoopDbContext.SaveChangesAsync();

            return new NoContentResult();
        }
    }
}
