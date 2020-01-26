using System.Collections.Generic;
using GraphQL.Models;
using GraphQL.Types;

namespace GraphQL.Queries
{
    [GraphQLMetadata("Query")]
    public class GraphQLQueryWrapper : ObjectGraphType
    {
        public GraphQLQueryWrapper(IDependencyResolver resolver)
        {            
            Field<ListGraphType<LessonType>>(
                "lessons",
                resolve : context => resolver.Resolve<ILessonRepository>().GetAll()
            );

            Field<LessonType>(
                "lesson",
                arguments : new QueryArguments(new QueryArgument<IntGraphType>{ Name = "id" }),
                resolve : context => resolver.Resolve<ILessonRepository>().GetById(context.GetArgument<int>("id"))
            );

            Field<ListGraphType<TeacherType>>(
                "teachers",
                resolve : context => resolver.Resolve<ITeacherRepository>().GetAll()
            );
        }
    }
}