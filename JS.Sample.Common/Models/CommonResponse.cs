using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace JS.Sample.Common.Models
{
    /// <summary>
    /// Common response model to be shared accross all requests
    /// </summary>
    public class CommonResponse
    {
        public CommonResponse()
        {
        }

        /// <summary>
        /// Assign the common response values
        /// </summary>
        /// <param name="message"></param>
        /// <param name="successful"></param>
        /// <param name="data"></param>
        /// <param name="statusCode"></param>
        public CommonResponse(string message, bool successful, object data = null, int statusCode = 0, string messageCode = "")
        {
            this.Message = message;
            this.MessageCode = messageCode;
            this.Successful = successful;
            this.Data = data;
            this.StatusCode = statusCode;
        }

        /// <summary>
        /// Assign the common response values
        /// </summary>
        /// <param name="message"></param>
        /// <param name="successful"></param>
        /// <param name="total"></param>
        /// <param name="data"></param>
        /// <param name="statusCode"></param>
        public CommonResponse(string message, bool successful, int? total, object data = null, int? pageSize = null, int? pageIndex = null, int statusCode = 0, string messageCode = "")
        {
            this.Message = message;
            this.MessageCode = messageCode;
            this.Successful = successful;
            this.Data = data;

            this.Total = total;
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;

            this.StatusCode = statusCode;
        }

        /// <summary>
        /// Assign the common response values
        /// </summary>
        /// <param name="message"></param>
        /// <param name="successful"></param>
        /// <param name="details"></param>
        /// <param name="statusCode"></param>
        public CommonResponse(string message, bool successful, string details, int statusCode = 0, string messageCode = "")
        {
            this.Message = message;
            this.MessageCode = messageCode;
            this.Successful = successful;
            this.Details = details;
            this.StatusCode = statusCode;
        }

        /// <summary>
        /// Assign the common response values
        /// </summary>
        /// <param name="modelState"></param>
        public CommonResponse(ModelStateDictionary modelState)
        {
            this.Successful = false;
            this.MessageCode = "";
            this.Message = "Validation Failed";
            this.Errors = modelState.Keys
                .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                .ToList();
        }

        /// <summary>
        /// List of model validation errors
        /// </summary>
        [JsonProperty("Errors", NullValueHandling = NullValueHandling.Ignore)]
        public List<ValidationError> Errors { get; set; }

        /// <summary>
        /// Details for the error triggered
        /// </summary>
        [JsonProperty("Details", NullValueHandling = NullValueHandling.Ignore)]
        public string Details { get; set; }

        /// <summary>
        /// Status of the request
        /// </summary>
        public bool Successful { get; set; }

        /// <summary>
        /// Message to be returned from the API
        /// </summary>
        public string Message { get; set; }
        public string MessageCode { get; set; }

        /// <summary>
        /// Custom status code to handle different types of
        /// success and failure responses
        /// </summary>
        public int StatusCode { get; set; } = 0;

        /// <summary>
        /// will contain the response data to be returned by api call
        /// </summary>
        [JsonProperty("Data", NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }

        /// <summary>
        /// will contain total number of records
        /// </summary>
        [JsonProperty("Total", NullValueHandling = NullValueHandling.Ignore)]
        public int? Total { get; set; }



        /// <summary>
        /// current page size
        /// </summary>
        [JsonProperty("PageSize", NullValueHandling = NullValueHandling.Ignore)]
        public int? PageSize { get; set; }

        /// <summary>
        /// current page index
        /// </summary>
        [JsonProperty("PageIndex", NullValueHandling = NullValueHandling.Ignore)]
        public int? PageIndex { get; set; }

        /// <summary>
        /// Total page count
        /// </summary>
        [JsonProperty("PageCount", NullValueHandling = NullValueHandling.Ignore)]
        public int? PageCount
        {
            get
            {
                if (Total == null || PageSize == null)
                    return null;

                return (int)Math.Ceiling((int)Total / (double)PageSize);
            }
        }

        /// <summary>
        /// Factory method to create success response
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <param name="statusCode"></param>
        /// <returns>CommonResponse</returns>
        public static CommonResponse CreateSuccessResponse(string message = "", object data = null, int statusCode = 0, string messageCode = "") => new CommonResponse(message, true, data, statusCode, messageCode);

        /// <summary>
        /// Factory method to create paging success response
        /// </summary>
        /// <param name="message"></param>
        /// <param name="total"></param>
        /// <param name="data"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static CommonResponse CreatePaginationResponse(string message = "", int? total = null, object data = null, int? pageSize = null, int? pageIndex = null, int statusCode = 0, string messageCode = "")
            => new CommonResponse(message, true, total, data, pageSize, pageIndex, statusCode, messageCode);

        /// <summary>
        /// Factory method to create success with details response
        /// </summary>
        /// <param name="message"></param>
        /// <param name="details"></param>
        /// <param name="statusCode"></param>
        /// <returns>CommonResponse</returns>
        public static CommonResponse CreateSuccessWithDetailsResponse(string message = "", string details = "", int statusCode = 0, string messageCode = "")
            => new CommonResponse(message, true, details, statusCode, messageCode);

        /// <summary>
        /// Factory method to create success failure response
        /// </summary>
        /// <param name="message"></param>
        /// <param name="statusCode"></param>
        /// <returns>CommonResponse</returns>
        public static CommonResponse CreateFailedResponse(string message = "", int statusCode = 0, string messageCode = "") => new CommonResponse(message, false, null, statusCode, messageCode);

        /// <summary>
        /// Factory method to create error response and populate with errors from model state
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns>CommonResponse</returns>
        public static CommonResponse CreateErrorResponse(ModelStateDictionary modelState) => new CommonResponse(modelState);
    }
}
