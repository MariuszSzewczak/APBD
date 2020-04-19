using Cw3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.DAL
{
    public class MockDbService : IDbService
    {
        private static IEnumerable<Student> _students = new List<Student>
        {
            new Student{ FirstName="Jan", LastName="Kowalski", IndexNumber="s1234"},
            new Student{FirstName="Anna", LastName="Malewski", IndexNumber="s2342"},
            new Student{FirstName="Krzysztof", LastName="Andrzejewicz", IndexNumber="s5432"}
        };

        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }
    }
}