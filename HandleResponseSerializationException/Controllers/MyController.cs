using Microsoft.AspNetCore.Mvc;
using System.Text;


namespace HandleResponseSerializationException.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MyController : ControllerBase
    {
        [HttpGet("error-in-response-serialization")]
        public RecursiveObj GetErrorInResponseSerialization(int fooContentLength)
        {
            var sb = new StringBuilder(fooContentLength);
            for (var i = 0; i < fooContentLength; i++)
            {
                sb.Append(".");
            }

            var obj = new RecursiveObj(sb.ToString());
            obj.Parent = obj;
            return obj;
        }

        public record RecursiveObj(string? FooData)
        {
            public RecursiveObj? Parent { get; set; }
        }
    }
}
