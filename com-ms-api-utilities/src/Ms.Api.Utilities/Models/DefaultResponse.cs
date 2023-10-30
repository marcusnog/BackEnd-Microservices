using Ms.Api.Utilities.Extensions;
using System.Text.Json.Serialization;

namespace Ms.Api.Utilities.Models
{
    public class DefaultResponse<T>
	{
		[JsonConstructor]
		public DefaultResponse()
		{
		}
		public DefaultResponse(bool success = true)
		{
			Success = success;
		}
		public DefaultResponse(T data, bool success = true) : this(success)
		{
			Data = data;
		}

		public DefaultResponse(Exception ex, string? messageCode = null) : this(ex.GetFullException(), messageCode)
		{
		}
		public DefaultResponse(string? erroValidacao, string? messageCode = null)
		{
			Success = false;
			MessageCode = messageCode;
			if (string.IsNullOrWhiteSpace(erroValidacao)) return;

			Errors = new List<string>() { erroValidacao };
		}

		public void AddError(string message)
        {
			Errors ??= new ();
			Errors.Add(message);
        }

		public bool Success { get; set; }
		public T? Data { get; set; }
		public string? MessageCode { get; set; }
		[JsonIgnore] string? message { get; set; }
		public string? Message { 
			get { return message ?? (Errors?.Any() != true ? null : string.Join("\n", Errors)); }  
			set { message = value; }
		}
		[JsonIgnore]
		List<string>? Errors { get; set; }
	}
}
