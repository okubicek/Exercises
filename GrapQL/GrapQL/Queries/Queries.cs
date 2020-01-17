using System.Collections.Generic;
using GraphQL.Models;
using GraphQL.Types;

namespace GraphQL.Queries
{
    [GraphQLMetadata("Query")]
    public class Queries : ObjectGraphType
    {
        private ILessonRepository _repo;

        public Queries(ILessonRepository lessonRepo, ITeacherRepository teacherRepo)
        {            
            _repo = lessonRepo;
            Field<ListGraphType<LessonType>>(
                "lessons",
                resolve : context => lessonRepo.GetAll()
            );

            Field<LessonType>(
                "lesson",
                arguments : new QueryArguments(new QueryArgument<IntGraphType>{ Name = "id" }),
                resolve : context => lessonRepo.GetById(context.GetArgument<int>("id"))
            );

            Field<ListGraphType<TeacherType>>(
                "teachers",
                resolve : context => teacherRepo.GetAll()
            );
        }
    }
}