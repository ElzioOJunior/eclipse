using Microsoft.AspNetCore.Mvc;

namespace EclipseApi.Controllers.Helpers
{
    public static class ResponseHelper
    {
        public static IActionResult SuccessResponse(object data, string message = "Operação concluída com sucesso")
        {
            return new OkObjectResult(new ApiResponse
            {
                Data = data,
                Status = "success",
                Message = message
            });
        }
        public static IActionResult ErrorResponse(string message = "Erro ao solicitar a operação")
        {
            return new OkObjectResult(new ApiResponse
            {
                Status = "error",
                Message = message
            });
        }
    }
}
