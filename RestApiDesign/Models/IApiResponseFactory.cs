namespace RestApiDesign.Models
{
    public interface IApiResponseFactory
    {
        ApiResponse<T> CreateSuccessResponse<T>(T data);
        ApiResponse<string> CreateErrorResponse(string errorMessage); 
    }
}
