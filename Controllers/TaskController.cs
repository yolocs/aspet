using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using dao = aspet.Daos;
using dto = aspet.Models;

namespace aspet.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ILogger<TaskController> _logger;
        private dao.ITasks tasks;

        public TaskController(dao.ITasks tasks, ILogger<TaskController> _logger)
        {
            this.tasks = tasks;
            this._logger = _logger;
        }

        [HttpPost]
        public void Create(dto.TaskText content)
        {
            var email = User.Claims.FirstOrDefault().Value;
            this.tasks.Create(email, content.Text);
        }

        [HttpPost]
        public void Complete(dto.TaskIdentifier id)
        {
            var email = User.Claims.FirstOrDefault().Value;
            this.tasks.Complete(email, id.TaskId);
        }

        [HttpPost]
        public void Incomplete(dto.TaskIdentifier id)
        {
            var email = User.Claims.FirstOrDefault().Value;
            this.tasks.Incomplete(email, id.TaskId);
        }

        [HttpPost]
        public void Archive(dto.TaskIdentifier id)
        {
            var email = User.Claims.FirstOrDefault().Value;
            this.tasks.Archive(email, id.TaskId);
        }
    }
}
