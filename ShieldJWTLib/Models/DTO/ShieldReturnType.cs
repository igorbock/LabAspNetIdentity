namespace ShieldJWTLib.Models.DTO
{
    public class ShieldReturnType
    {
        public string Message { get; set; }
        public bool? IsError { get; set; }
        public int Code { get; set; }

        public ShieldReturnType()
        {
            IsError = false;
            Code = 200;
        }

        public ShieldReturnType(string message)
        {
            Message = message;
            IsError = false;
            Code = 200;
        }

        public ShieldReturnType(string message, int code)
        {
            Message = message;
            IsError = true;
            Code = code;
        }
    }
}
