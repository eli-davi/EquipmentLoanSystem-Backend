using EquipmentLoanSystem.Core.DTOs;
using EquipmentLoanSystem.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentLoanSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            // בדיקת דמו פשוטה לחיבור
            if (dto.Email == "admin@example.com" && dto.Password == "123456")
            {
                var token = _authService.GenerateJwtToken("1", dto.Email, "Admin");
                return Ok(new AuthResponseDto { Token = token, Email = dto.Email, Role = "Admin" });
            }

            if (dto.Email == "user@example.com" && dto.Password == "123456")
            {
                var token = _authService.GenerateJwtToken("2", dto.Email, "User");
                return Ok(new AuthResponseDto { Token = token, Email = dto.Email, Role = "User" });
            }

            return Unauthorized(new { message = "שם משתמש או סיסמה שגויים." });
        }
    }
}