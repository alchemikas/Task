using System.Net;

namespace Product.Api.Client
{
    public class ApiResponse<T>
    {
        public HttpStatusCode HttpStatusCode{ get; set; }
        public T Response { get; set; }
    }

    public class ApiResponse
    {
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
