using GraphQL.Models;
using GraphQL.Types;

public class LessonType : ObjectGraphType<Lesson>
{
    public LessonType(ITeacherRepository repo)
    {
        Field(x => x.Id);
        Field(x => x.Name);
        Field<TeacherType>("teacher", 
            arguments : new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
            resolve: context => repo.GetById(context.Source.Id)
        );
    }
}