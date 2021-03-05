using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

using dto = aspet.Models;

namespace aspet.Daos
{
    public class InMemoryTasks : ITasks
    {
        private ConcurrentDictionary<int, dto.Task> dict;
        private int nextId;

        public InMemoryTasks()
        {
            this.dict = new ConcurrentDictionary<int, dto.Task>();
            this.nextId = 0;
            // Add fake data.
            this.Create("shou3301@gmail.com", "My first todo");
            this.Create("shou3301@gmail.com", "Buy milk");
        }

        public void Archive(string email, int id)
        {
            this.dict.TryRemove(id, out _);
        }

        public void Complete(string email, int id)
        {
            this.dict[id].Completed = DateTime.Now;
        }

        public void Create(string email, string text)
        {
            var id = Interlocked.Increment(ref this.nextId);
            var t = new dto.Task()
            {
                Email = email,
                TaskId = id,
                Text = text
            };
            this.dict[id] = t;
        }

        public void Incomplete(string email, int id)
        {
            this.dict[id].Completed = null;
        }

        public List<dto.Task> List(string email)
        {
            List<dto.Task> result = new List<dto.Task>();
            foreach (KeyValuePair<int, dto.Task> entry in this.dict)
            {
                if (entry.Value.Email == email)
                {
                    result.Add(entry.Value);
                }
            }
            return result;
        }
    }
}
