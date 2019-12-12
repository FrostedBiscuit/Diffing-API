using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

using Newtonsoft.Json;

using DiffingAPI.DiffLogic;

namespace DiffingAPI.Controllers
{
    public class diffController : ApiController {

        // GET: v1/diff/{id}
        // Tries to run a comparison at the id.
        // Responds with result in JSON if the comparison is valid.
        // If the comparison is not valid, it responds with a code 404 (not found). 
        [HttpGet]
        public HttpResponseMessage GetById(int id) {

            HttpResponseMessage comparisonResponse = new HttpResponseMessage();

            DiffResult result = DiffManager.Compare(id);

            if (result.Valid == false) {

                comparisonResponse.StatusCode = HttpStatusCode.NotFound;

                return comparisonResponse;
            }

            comparisonResponse.Content = new StringContent(JsonConvert.SerializeObject(result));
            comparisonResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            comparisonResponse.StatusCode = HttpStatusCode.OK;

            return comparisonResponse;
        }

        // PUT: v1/diff/{id}/left
        // Adds left part of entry, if the data is null
        // it responds with a code 400 (bad request)
        [HttpPut]
        public HttpResponseMessage left(int id, DataEntry leftEntry) {

            if (leftEntry.Data == null) {

                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            DiffManager.AddLeftValue(id, leftEntry.Data);

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        // PUT: v1/diff/{id}/right
        // Adds right part of entry, if the data is null
        // it responds with a code 400 (bad request)
        [HttpPut]
        public HttpResponseMessage right(int id, DataEntry rightEntry) {

            if (rightEntry.Data == null) {

                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            DiffManager.AddRightValue(id, rightEntry.Data);

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        public class DataEntry {

            public string Data {
                get {
                    return _data;
                }
                set {
                    _data = value == string.Empty ? null : value;
                }
            }

            private string _data;
        }
    }
}
