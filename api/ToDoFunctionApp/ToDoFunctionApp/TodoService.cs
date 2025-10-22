using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoFunctionApp.Models;

namespace ToDoFunctionApp
{
    public static class TodoService
    {
        private static List<Todo> todos = new List<Todo>
        {
            new Todo { Id = 1, Task = "Learn Azure" }
        };

        public static List<Todo> GetAll() => todos;

        public static Todo Add(string task)
        {
            var todo = new Todo { Id = todos.Count + 1, Task = task };
            todos.Add(todo);
            return todo;
        }
    }
}
