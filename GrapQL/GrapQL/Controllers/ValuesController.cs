using Microsoft.AspNetCore.Mvc;
using GraphQL;
using System.Threading.Tasks;
using GraphQL.Queries;
using GraphQL.Types;
namespace GraphQL.Controllers
{
    [Route("graphql")]
    [ApiController]
    public class GraphQlController : ControllerBase
    {
        private LessonSchema _schema;

        public GraphQlController(LessonSchema schema)
        {
            _schema = schema;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GraphQLQuery query)
        {
            var result = await new DocumentExecuter().ExecuteAsync( x => {
                x.Schema = _schema.Schema;
                x.Query = query.Query;
                x.OperationName = query.OperationName;
                x.Inputs = query.Variables.ToInputs();
            });

            return Ok(result);
        }
    }
}
