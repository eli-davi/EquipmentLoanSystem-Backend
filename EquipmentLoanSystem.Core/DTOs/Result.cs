namespace EquipmentLoanSystem.Core.DTOs
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public string? ErrorMessage { get; }
        public int ErrorCode { get; }

        private Result(bool isSuccess, T? value, string? errorMessage, int errorCode)
        {
            IsSuccess = isSuccess;
            Value = value;
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }

        public static Result<T> Success(T value) => new Result<T>(true, value, null, 200);
        public static Result<T> Fail(string errorMessage, int errorCode = 400) => new Result<T>(false, default, errorMessage, errorCode);
        public static Result<T> Conflict(string errorMessage) => new Result<T>(false, default, errorMessage, 409);
    }
}