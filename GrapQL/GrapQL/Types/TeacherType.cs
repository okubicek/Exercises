using GraphQL.Models;
using GraphQL.Types;

public class TeacherType : ObjectGraphType<Teacher>
{
    public TeacherType()
    {
        Field(x => x.Id);
        Field(x => x.FirstName);
        Field(x => x.SecondName);
    }
}