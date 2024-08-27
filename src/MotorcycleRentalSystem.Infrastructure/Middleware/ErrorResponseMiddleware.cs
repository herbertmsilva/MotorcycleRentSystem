using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MotorcycleRentalSystem.Application.Exceptions;
using MotorcycleRentalSystem.Application.Responses;
using MotorcycleRentalSystem.Core.Entities.Mongo;
using MotorcycleRentalSystem.Core.Interfaces;
using System.Net;
using System.Text.Json;

public class ErrorResponseMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorResponseMiddleware> _logger;
    private readonly ILogRepository _logRepository;

    public ErrorResponseMiddleware(RequestDelegate next, ILogger<ErrorResponseMiddleware> logger, ILogRepository logRepository)
    {
        _next = next;
        _logger = logger;
        _logRepository = logRepository;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BusinessValidationException ex)
        {
            _logger.LogWarning(ex, "Erro de validação de negócio ocorreu.");
            await HandleBusinessValidationExceptionAsync(context, ex);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Recurso não encontrado.");
            await HandleNotFoundExceptionAsync(context, ex);
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "Erro de validação ocorreu.");
            var logId = await InserLogIntoDB(ex);
            await HandleValidationExceptionAsync(context, ex, logId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro inesperado.");
            var logId = await InserLogIntoDB(ex);
            await HandleExceptionAsync(context, ex, logId);
        }
    }

    private async Task<Guid> InserLogIntoDB(Exception ex)
    {
        var logEntry = new LogEntry { Exception = ex.ToString(), Level = MotorcycleRentalSystem.Core.Enums.LogLevel.Error, Message = ex.Message, Source = ex.Source, Timestamp = DateTime.Now };
        await _logRepository.AddLogAsync(logEntry);
        return logEntry.Id;
    }

    private static Task HandleBusinessValidationExceptionAsync(HttpContext context, BusinessValidationException exception)
    {
        var response = new ApiResponse<object>
        {
            Success = false,
            Message = exception.Message,
            Data = null,
            Errors = new List<ApiError>()
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private Task HandleNotFoundExceptionAsync(HttpContext context, NotFoundException exception)
    {
        var response = new ApiResponse<object>
        {
            Success = false,
            Message = exception.Message,
            Data = null,
            Errors = new List<ApiError>()
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception, Guid errorId)
    {
        var errors = exception.Errors.Select(e => new ApiError(errorId, e.PropertyName, e.ErrorMessage)).ToList();

        var response = new ApiResponse<object>
        {
            Success = false,
            Message = "A validação falhou para um ou mais campos.",
            Data = null,
            Errors = errors
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception, Guid errorId)
    {
        var response = new ApiResponse<object>
        {
            Success = false,
            Message = "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.",
            Data = null,
            Errors = new List<ApiError> { new ApiError(errorId, "Exceção", exception.Message) }
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
