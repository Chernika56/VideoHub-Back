using BackEnd.Organizations.DTO.RequestDTO;
using BackEnd.Organizations.Services;
using BackEnd.Utils.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Organizations.Enpoints
{
    public static class OrganizationUsersEndpoints
    {
        public static void MapOrganizationUsers(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/{organizationId:int}/users", GetOrganizationUsers)
                .WithOpenApi();
            builder.MapGet("/{organizationId:int}/users/{userId:int}", GetOrganizationUser)
                .WithOpenApi();
            builder.MapPut("/{organizationId:int}/users/{userId:int}", ChangeOrganizationUser)
                .WithOpenApi();
            builder.MapDelete("/{organizationId:int}/users/{userId:int}", DeleteUserFromOrganization)
                .WithOpenApi();
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> GetOrganizationUsers([FromServices] OrganizationUsersService service, int organizationId)
        {
            var users = await service.GetOrganizationUsers(organizationId);

            return users is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(users);
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> GetOrganizationUser([FromServices] OrganizationUsersService service, int organizationId, int userId)
        {
            var user = await service.GetOrganizationUser(organizationId, userId);

            return user is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(user);
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> ChangeOrganizationUser([FromServices] OrganizationUsersService service, [FromBody] OrganizationUserRequestDTO dto, int organizationId, int userId)
        {
            var user = await service.ChangeOrganizationUser(dto, organizationId, userId);

            return user is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(user);
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> DeleteUserFromOrganization([FromServices] OrganizationUsersService service, int organizationId, int userId)
        {
            var res = await service.DeleteUserFromOrganization(organizationId, userId);

            return res is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
