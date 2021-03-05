using System;

namespace aspet.Models
{
    public class Task
    {
        public int TaskId { get; set; }

        public string Email { get; set; }

        public string Text { get; set; }

        public DateTime? Completed { get; set; }
    }
}
