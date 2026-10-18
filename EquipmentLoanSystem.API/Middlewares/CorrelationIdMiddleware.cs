using NLog;

namespace EquipmentLoanSystem.API.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // יצירת מזהה ייחודי לבקשה
            var correlationId = Guid.NewGuid().ToString();
            
            // הוספת המזהה ל-Headers כדי שגם הלקוח יראה אותו
            context.Response.Headers.Append("X-Correlation-ID", correlationId);

            // דחיפת המזהה לתוך ההקשר של NLog
            using (ScopeContext.PushProperty("CorrelationId", correlationId))
            {
                await _next(context);
            }
        }
    }
}