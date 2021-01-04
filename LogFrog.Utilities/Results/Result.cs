namespace LogFrog.Utilities.Results
{
    public class Result<T>
    {
        public bool IsOk { get; private set; }
        public bool IsFail => !IsOk;

        private T value;
        public T Value => IsOk ? value : throw new ResultException("Unable to get value when result is failed");

        private string errorMessage;
        public string ErrorMessage => IsFail ? errorMessage : throw new ResultException("Unable to error message when result is ok");
        private Result() {}

        public static Result<T> Ok(T value) => new()
        {
            IsOk = true,
            value = value
        };

        public static Result<T> Fail(string errorMessage) => new()
        {
            IsOk = false,
            errorMessage = errorMessage
        };
        
        public static implicit operator string(Result<T> result)
        {
            return result.ErrorMessage;
        }
        
        public static implicit operator T(Result<T> result)
        {
            return result.Value;
        }
    }
}