using Microsoft.AspNetCore.Mvc;
using NetFilm.Application.Exceptions;
using System.Net;

namespace NetFilm.API.Middlewares
{
	public class ExceptionHandlerMiddleware
	{
		private readonly RequestDelegate next;

		public ExceptionHandlerMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await next(context);
			}
			catch (Exception exception)
			{
				await HandleException(context, exception);
			}
		}

		private async Task HandleException(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";
			if (exception is NotFoundException)
			{
				context.Response.StatusCode = (int)HttpStatusCode.NotFound;
			}
			else if (exception is ExistedEntityException || exception is FileNotAllowException)
			{
				context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
			}
			else
			{
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			}

			var errorResponse = new ProblemDetails
			{
				Title = "Some things went wrong!",
				Detail = exception.Message
			};

			await context.Response.WriteAsJsonAsync(errorResponse);
		}
	}
}
