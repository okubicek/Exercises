using GraphQL;
using GraphQL.Queries;
using GraphQL.Types;

public class LessonSchema : Schema
{
    public LessonSchema(
        IDependencyResolver resolver) : base(resolver)
    {
        Query = resolver.Resolve<GraphQLQueryWrapper>();
    }

    // public ISchema Schema {get; private set;}

    // private ISchema _schema
    // {
    //     get {
    //         return GraphQL.Types.Schema.For(
    //             @"type Lesson{
    //                 id: ID,
    //                 name: String,
    //                 teacher: Teacher
    //             }
                
    //             type Teacher{
    //                 id: ID,
    //                 firstName: String,
    //                 secondName: String
    //             }

    //             type Query{
    //                 lessons: [Lesson],
    //                 lesson(id:ID): Lesson,
    //                 teachers: [Teacher]
    //             }
    //             ", x => { x.Types.Include<Queries>();}
    //         );
    //     }
    // }
}