namespace RestApiDesign.Models
{
    public class ApiResponseFactory : IApiResponseFactory
    {
        public ApiResponse<T> CreateSuccessResponse<T>(T data)
        {
            return new ApiResponse<T>
            {
                Data = data
            };
        }

        public ApiResponse<string> CreateErrorResponse(string errorMessage)
        {
            return new ApiResponse<string>
            {
                ErrorMessage = errorMessage,
                ErrorCode = "BadRequest"
            };
        }
    }
}
