using BackEnd.Authorization.Services;
using BackEnd.DB.Context;
using BackEnd.DB.Entities;
using BackEnd.Messages.DTO;
using BackEnd.Users.Services;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Messages.Services
{
    public class MessageService(ILogger<UserService> logger, MyDbContext db, AuthHelper authHelper)
    {
        public async Task<List<MessageResponseDTO>?> GetMessages()
        {
            try
            {
                var messages = await db.Messages
                    .Include(m => m.User)
                    .Include(m => m.Sender)
                    .Select(m => new MessageResponseDTO
                    {
                        Id = m.Id,
                        User = new User
                        {
                            Id = m.User.Id,
                            Login = m.User.Login
                        },
                        Sender = new User
                        {
                            Id = m.Sender.Id,
                            Login = m.Sender.Login
                        },
                        Title = m.Title,
                        Body = m.Body,
                        Type = m.Type,
                        IsPush = m.IsPush,
                        IsDashboard = m.IsDashboard,
                        IsDeleted = m.IsDeleted,
                        WasRead = m.WasRead
                    }).ToListAsync();

                return messages;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during message finding {exception}", ex);
                return null;
            }
        }

        public async Task<MessageResponseDTO?> GetMessage(int messageId)
        {
            try
            {
                var message = await db.Messages
                    .Include(m => m.User)
                    .Include(m => m.Sender)
                    .Where(m => m.Id == messageId)
                    .Select(m => new MessageResponseDTO
                    {
                        Id = m.Id,
                        User = new User
                        {
                            Id = m.User.Id,
                            Login = m.User.Login
                        },
                        Sender = new User
                        {
                            Id = m.Sender.Id,
                            Login = m.Sender.Login
                        },
                        Title = m.Title,
                        Body = m.Body,
                        Type = m.Type,
                        IsPush = m.IsPush,
                        IsDashboard = m.IsDashboard,
                        IsDeleted = m.IsDeleted,
                        WasRead = m.WasRead
                    }).FirstOrDefaultAsync();

                return message;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during message finding {exception}", ex);
                return null;
            }
        }

        public async Task<MessageResponseDTO?> SendMessage(MessageSendDTO dto)
        {
            try
            {
                var sender = await db.Users.FirstOrDefaultAsync(u => u.Login == authHelper.GetCurrentUserLogin());

                if (sender is null)
                {
                    throw new Exception("User not found");
                }

                var message = new MessagesEntity
                {
                    UserId = dto.UserId,
                    SenderId = sender.Id,
                    Title = dto.Title,
                    Body = dto.Body,
                    Type = dto.Type,
                    IsPush = dto.IsPush ?? false,
                    IsDashboard = dto.IsDashboard ?? false,
                    IsDeleted = false,
                    WasRead = false,
                };

                db.Messages.Add(message);
                await db.SaveChangesAsync();

                return await GetMessage((int)message.Id);
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during message finding {exception}", ex);
                return null;
            }
        }

        public async Task<bool?> DeleteMessage(int messageId)
        {
            try
            {
                var message = await db.Messages.FindAsync(messageId);

                if (message is null)
                {
                    return false;
                }

                db.Messages.Remove(message);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during message finding {exception}", ex);
                return null;
            }
        }
    }
}
