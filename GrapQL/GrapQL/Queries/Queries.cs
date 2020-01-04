using System.Collections.Generic;
using GraphQL.Models;

namespace GraphQL.Queries
{
    [GraphQLMetadata("Query")]
    public class Queries //: ObjectGraphType
    {
        // private ILessonRepository _repo;

        // public Queries(ILessonRepository repo)
        // {            
        //     _repo = repo;
        // }

        [GraphQLMetadata("lessons")]
        public IEnumerable<Lesson> GetLessons()
        {
            return new LessonRepository().GetAll();
        }

        [GraphQLMetadata("lesson")]
        public Lesson GetLesson(int id)
        {
            return null;//_repo.GetById(id);
        }

        [GraphQLMetadata("teachers")]
        public IEnumerable<Teacher> GetTeachers()
        {
            return null;
        }
    }
}