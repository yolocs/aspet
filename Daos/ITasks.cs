using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dto = aspet.Models;

namespace aspet.Daos
{
    public interface ITasks
    {
        void Create(string email, string text);
        void Complete(string email, int id);
        void Incomplete(string email, int id);
        void Archive(string email, int id);

        List<dto.Task> List(string email);
    }
}
