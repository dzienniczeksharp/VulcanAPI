using Newtonsoft.Json;

namespace VulcanAPI
{
    public class ApiResponse<T>
    {
        public static ApiResponse<T> FailedReponse => new ApiResponse<T>() {Success = false};

        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("feedback")]
        public Feedback Feedback { get; set; }
    }

    public class Feedback
    {
        [JsonProperty("Handled")]
        public bool handled;

        [JsonProperty("FType")]
        public string type;

        [JsonProperty("Message")]
        public string message;

        [JsonProperty("ExceptionMessage")]
        public string exceptionMessage;

        [JsonProperty("InnerExceptionMessage")]
        public string innerExceptionMessage;

        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
