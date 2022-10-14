
namespace GreenSystem.Charging.Abstractions
{
    public class ReturnResult
    {
        public static readonly ReturnResult SuccessResult = new ReturnResult() { };

        protected ReturnResult()
        { }

        protected ReturnResult(string code, string description = null)
        {
            this.Code = code;
            this.Description = description;
        }


        public string Code
        {
            get;
        }

        public string Description
        {
            get;
        }

        public bool Success
        {
            get
            {
                return this.Code == null;
            }
        }

        public static ReturnResult ErrorResult(string code, string description = null)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new System.ArgumentException($"'{nameof(code)}' cannot be null or whitespace", nameof(code));
            }

            return new ReturnResult(code, description);
        }
    }

    public class ReturnResult<T> : ReturnResult
    {
        private ReturnResult(string code, string description = null) : base(code, description)
        {
        }

        private ReturnResult(T result)
        {
            this.Result = result;
        }

        public T Result
        {
            get;
        }

        public static new ReturnResult<T> SuccessResult(T value)
        {
            return new ReturnResult<T>(value);
        }

        public static new ReturnResult<T> ErrorResult(string code, string description)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new System.ArgumentException($"'{nameof(code)}' cannot be null or whitespace", nameof(code));
            }

            return new ReturnResult<T>(code, description);
        }
    }
}
