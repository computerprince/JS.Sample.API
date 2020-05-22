using Newtonsoft.Json;

namespace JS.Sample.Common.Models
{
    /// <summary>
    /// Validation error model to hold all the validation errors 
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// Fields on which there is issue
        /// </summary>
        [JsonProperty("Errors", NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }

        /// <summary>
        /// Message to be shown to the client
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Add the validation errors
        /// </summary>
        /// <param name="field"></param>
        /// <param name="message"></param>
        public ValidationError(string field, string message)
        {
            Field = field != string.Empty ? field : null;
            Message = message;
        }


        public static ValidationError CreateFailedResponse(string field = "501", string message = "") => new ValidationError(field, message);
        public static ValidationError CreateSuccessResponse(string field = "200", string message = "") => new ValidationError(field, message);
    }
}
