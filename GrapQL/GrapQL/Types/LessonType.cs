using GraphQL;
using GraphQL.Models;
using GraphQL.Types;

public class LessonType : ObjectGraphType<Lesson>
{
    public LessonType(IDependencyResolver resolver)
    {
        Field(x => x.Id);
        Field(x => x.Name);
        Field<TeacherType>("teacher", 
            arguments : new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
            resolve: context => resolver.Resolve<ITeacherRepository>().GetById(context.Source.Id)
        );
    }
}