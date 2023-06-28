namespace TemplateDapper.Application.Common.Responses;

public enum StatusCode
{
    Success = 200,
    Created = 201,
    ModelValidation = 400,
    Forbidden = 403,
    NotFound = 404,
    BusinessLogicValidation = 409,
    Unhandled = 500
}
