using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ToDoList.API.Data;
using ToDoList.API.Models;

namespace ToDoList.API.Authorization
{
    public class IsOpenLoopOwnerHandler :
    AuthorizationHandler<IsOpenLoopOwnerRequirement, OpenLoop>
    {
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            IsOpenLoopOwnerRequirement requirement,
            OpenLoop resource)
        {
            var userId = context.User.Claims?.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (userId.IsNullOrEmpty())
            {
                return;
            }

            if (resource.CreatorId == userId)
            {
                context.Succeed(requirement);
            }
        }
    }

}
