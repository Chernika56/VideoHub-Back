using BackEnd.Organizations.DTO.RequestDTO;
using BackEnd.Organizations.Services;
using BackEnd.Utils.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Organizations.Enpoints
{
    public static class OrganizationEndpoints
    {
        public static void MapOrganizations(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/", GetOrganizations)
                .WithOpenApi();
            builder.MapPost("/", CreateOrganization)
                .WithOpenApi();
            builder.MapGet("/{organizationId:uint}", GetOrganization)
                .WithOpenApi();
            builder.MapPut("/{organizationId:uint}", ChangeOrganization)
                .WithOpenApi();
            builder.MapDelete("/{organizationId:uint}", DeleteOrganization)
                .WithOpenApi();
        }

        [Authorize(Policy = PolicyType.AdministratorPolicy)]
        private static async Task<IResult> GetOrganizations([FromServices] OrganizationService service)
        {
            var organizations = await service.GetOrganizations();

            return organizations is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(organizations);
        }

        [Authorize(Policy = PolicyType.AdministratorPolicy)]
        private static async Task<IResult> CreateOrganization([FromServices] OrganizationService service, [FromBody] OrganizationRequestDTO dto)
        {
            var organization = await service.CreateOrganization(dto);

            return organization is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(organization);
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> GetOrganization([FromServices] OrganizationService service, uint organizationId)
        {
            var organization = await service.GetOrganization(organizationId);

            return organization is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(organization);
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> ChangeOrganization([FromServices] OrganizationService service, [FromBody] OrganizationRequestDTO dto, uint organizationId)
        {
            var organization = await service.ChangeOrganization(dto, organizationId);

            return organization is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(organization);
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> DeleteOrganization([FromServices] OrganizationService service, uint organizationId)
        {
            var res = await service.DeleteOrganization(organizationId);

            return res is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
