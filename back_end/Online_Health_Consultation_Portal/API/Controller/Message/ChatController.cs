using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Dtos.Message;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Health_Consultation_Portal.API.Controller.Message
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ChatController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET /api/chat/users?excludeId={currentUserId}
        /// Lấy tất cả user làm contact, ngoại trừ chính user hiện tại.
        /// Trả về: Id, FullName, AvatarUrl (tuỳ bạn có thêm các trường status: online/offline không).
        /// </summary>
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers([FromQuery] int excludeId)
        {
            // Kiểm tra nếu excludeId <= 0 => BadRequest
            if (excludeId <= 0)
                return BadRequest("Chưa cung cấp hoặc sai định dạng excludeId.");

            // Lấy danh sách user (trừ user có Id = excludeId)
            var users = await _context.Users
                .Where(u => u.Id != excludeId)
                .Select(u => new
                {
                    u.Id,
                    u.FullName,
                    u.ImageUrl
                    // Nếu bạn có thêm cột status (online/offline), có thể map vào đây luôn.
                })
                .ToListAsync();

            return Ok(users);
        }

        /// <summary>
        /// GET /api/chat/messages?user1={user1}&user2={user2}
        /// Lấy lịch sử tin nhắn giữa 2 user (sắp xếp theo thời gian tăng dần).
        /// </summary>
        [HttpGet("messages")]
        public async Task<IActionResult> GetMessagesBetween(
            [FromQuery] int user1,
            [FromQuery] int user2)
        {
            if (user1 <= 0 || user2 <= 0)
                return BadRequest("Thiếu hoặc sai định dạng user1 hoặc user2.");

            // Lấy tất cả message giữa user1 và user2
            var messages = await _context.Messages
                .Where(m =>
                    m.SenderId == user1 && m.ReceiverId == user2 ||
                    m.SenderId == user2 && m.ReceiverId == user1)
                .OrderBy(m => m.SentAt)
                .Select(m => new MessageDto
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    Content = m.Content,
                    SentAt = m.SentAt,
                    IsRead = m.IsRead,
                    ReadAt = m.ReadAt
                })
                .ToListAsync();

            return Ok(messages);
        }

        /// <summary>
        /// POST /api/chat/messages
        /// Body (JSON): { "senderId": x, "receiverId": y, "content": "Nội dung" }
        /// Gửi tin nhắn mới và lưu vào database.
        /// </summary>
        [HttpPost("messages")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Kiểm tra user tồn tại?
            var sender = await _context.Users.FindAsync(dto.SenderId);
            var receiver = await _context.Users.FindAsync(dto.ReceiverId);
            if (sender == null || receiver == null)
                return NotFound("Sender hoặc Receiver không tồn tại.");

            // Tạo entity Message mới
            var message = new Message
            {
                SenderId = dto.SenderId,
                ReceiverId = dto.ReceiverId,
                Content = dto.Content,
                SentAt = DateTime.UtcNow,
                IsRead = false,
                ReadAt = null
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            // Trả về MessageDto
            var result = new MessageDto
            {
                Id = message.Id,
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId,
                Content = message.Content,
                SentAt = message.SentAt,
                IsRead = message.IsRead,
                ReadAt = message.ReadAt
            };

            // CREATED (201) kèm location header (tạm thời return Created tại chính GET messages giữa 2 user)
            return CreatedAtAction(nameof(GetMessagesBetween),
                new { user1 = dto.SenderId, user2 = dto.ReceiverId },
                result);
        }

        /// <summary>
        /// (Tuỳ chọn) Đánh dấu tin nhắn đã đọc:
        /// PUT /api/chat/messages/{messageId}/read
        /// </summary>
        [HttpPut("messages/{messageId}/read")]
        public async Task<IActionResult> MarkAsRead(int messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if (message == null)
                return NotFound();

            message.IsRead = true;
            message.ReadAt = DateTime.UtcNow;
            _context.Messages.Update(message);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
