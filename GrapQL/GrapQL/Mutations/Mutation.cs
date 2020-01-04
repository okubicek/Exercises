using GraphQL.Models;

namespace GraphQL.Mutations
{
    public class Mutation
    {
        [GraphQLMetadata("AddLesson")]
        public Lesson Add(string name, Teacher teacher)
        {
            return null;            
        }
    }
}