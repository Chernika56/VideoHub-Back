
using BackEnd.Messages.DTO;
using BackEnd.Messages.Services;
using BackEnd.Mosaics.DTO;
using BackEnd.Mosaics.Services;
using BackEnd.Utils.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Messages.Endpoints
{
    public static class MessagesEndpoints
    {
        public static void MapMessages(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/", GetMessages)
                .WithOpenApi();
            builder.MapGet("/{messageId:int}", GetMessage)
                .WithOpenApi();
            builder.MapPost("/", SendMessage)
                .WithOpenApi();
            builder.MapDelete("/{messageId:int}", DeleteMessage)
                .WithOpenApi();
        }

        [Authorize(Policy = PolicyType.AdministratorPolicy)]
        private async static Task<IResult> GetMessages([FromServices] MessageService service)
        {
            var messages = await service.GetMessages();

            return messages is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(messages);
        }

        [Authorize(Policy = PolicyType.AdministratorPolicy)]
        private async static Task<IResult> GetMessage([FromServices] MessageService service, int messageId)
        {
            var message = await service.GetMessage(messageId);

            return message is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(message);
        }

        [Authorize]
        private async static Task<IResult> SendMessage([FromServices] MessageService service, MessageSendDTO dto)
        {
            var message = await service.SendMessage(dto);

            return message is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(message);
        }

        [Authorize(Policy = PolicyType.AdministratorPolicy)]
        private async static Task<IResult> DeleteMessage([FromServices] MessageService service, int messageId)
        {
            var res = await service.DeleteMessage(messageId);

            return res is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
