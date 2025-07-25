namespace HomeZone.Services.Wallet.Wrappers
{
    
 
        public class ResponseWrapper<T>
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public T Data { get; set; }

            public ResponseWrapper(T data, string message = "", bool success = true)
            {
                Success = success;
                Message = message;
                Data = data;
            }
        }
    

}
